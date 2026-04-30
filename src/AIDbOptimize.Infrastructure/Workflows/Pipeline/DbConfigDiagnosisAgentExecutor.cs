using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Security;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Db config diagnosis agent executor.
/// </summary>
public sealed class DbConfigDiagnosisAgentExecutor(
    DbConfigDiagnosisAgentOptions options,
    ILoggerFactory loggerFactory,
    IServiceProvider services)
    : IDbConfigDiagnosisAgentExecutor
{
    private readonly DbConfigDiagnosisAgentOptions _options = options;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;
    private readonly IServiceProvider _services = services;
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public string PromptVersion => _options.PromptVersion;

    public string ModelId => _options.Model;

    public string BuildPrompt(
        string displayName,
        string databaseName,
        string engine,
        string optimizationGoal,
        DbConfigEvidencePack evidence)
    {
        return PromptInputBuilder.BuildDbConfigPrompt(
            displayName,
            databaseName,
            engine,
            optimizationGoal,
            notes: null,
            evidence);
    }

    public async Task<DbConfigDiagnosisExecutionResult> ExecuteAsync(
        DbConfigEvidencePack evidence,
        string? optimizationGoal,
        CancellationToken cancellationToken = default)
    {
        using var activity = AIDbOptimizeTelemetry.AgentActivitySource.StartActivity("agent.diagnosis.execute");
        activity?.SetTag("agent.model", _options.Model);
        activity?.SetTag("agent.endpoint", _options.Endpoint);

        var prompt = BuildPrompt(
            displayName: evidence.DatabaseName,
            databaseName: evidence.DatabaseName,
            engine: evidence.Engine.ToString(),
            optimizationGoal: optimizationGoal ?? string.Empty,
            evidence: evidence);

        var agent = BuildAgent(_options, _loggerFactory, _services);
        var session = await agent.CreateSessionAsync(cancellationToken);
        var response = await agent.RunAsync(
            prompt,
            session,
            cancellationToken: cancellationToken);

        var responseText = response.Text?.Trim();
        if (string.IsNullOrWhiteSpace(responseText))
        {
            throw new InvalidOperationException("MAF 诊断智能体没有返回内容。");
        }

        var value = JsonSerializer.Deserialize<DbConfigDiagnosisAgentResponse>(responseText, SerializerOptions)
            ?? throw new InvalidOperationException("MAF 诊断智能体返回的 JSON 无法解析。");
        EnforceNoSpecificTargetValuesWithoutHostContext(evidence, value);

        var reportJson = JsonSerializer.Serialize(new
        {
            title = value.Title,
            summary = value.Summary,
            recommendations = value.Recommendations,
            evidenceItems = value.EvidenceItems,
            missingContextItems = value.MissingContextItems ?? evidence.MissingContextItems,
            collectionMetadata = value.CollectionMetadata ?? evidence.CollectionMetadata,
            warnings = value.Warnings
        }, SerializerOptions);

        return new DbConfigDiagnosisExecutionResult(
            reportJson,
            """{"promptTokens":0,"completionTokens":0,"totalTokens":0}""");
    }

    private static ChatClientAgent BuildAgent(
        DbConfigDiagnosisAgentOptions options,
        ILoggerFactory loggerFactory,
        IServiceProvider services)
    {
        if (!options.IsConfigured)
        {
            throw new InvalidOperationException("DbConfig 诊断智能体尚未配置。");
        }

        var client = new ChatClient(
            model: options.Model,
            credential: new ApiKeyCredential(options.ApiKey),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri(options.Endpoint)
            });

        var meaiClient = client.AsIChatClient();
        return (ChatClientAgent)meaiClient.AsAIAgent(
            new ChatClientAgentOptions
            {
                Name = "DbConfigDiagnosisAgent",
                Description = "Generate a grounded db configuration optimization report.",
                ChatOptions = BuildChatOptions(options)
            },
            loggerFactory,
            services);
    }

    private static ChatOptions BuildChatOptions(DbConfigDiagnosisAgentOptions options)
    {
        return new ChatOptions
        {
            ResponseFormat = ShouldUseJsonResponseFormat(options)
                ? Microsoft.Extensions.AI.ChatResponseFormat.Json
                : Microsoft.Extensions.AI.ChatResponseFormat.ForJsonSchema<DbConfigDiagnosisAgentResponse>(SerializerOptions),
            Instructions = """
你是数据库配置优化分析助手。
你只能使用输入中的证据，不允许虚构环境信息或补全不存在的观测数据。
你必须只返回 JSON，并且 title、summary、suggestion 等自然语言字段必须使用中文。
如果缺少宿主资源上下文，不允许给出具体目标参数值或“调到 X GB / X MB / X%”这类结论，只能给出保守建议。
JSON 对象必须包含：
- title: string
- summary: string
- recommendations: array of { key: string, suggestion: string, severity: string, findingType: string, confidence: string, requiresMoreContext: boolean, impactSummary: string | null, evidenceReferences: string[], recommendationClass: string, appliesWhen: string | null, ruleId: string | null, ruleVersion: string | null }
- evidenceItems: array of { sourceType: string, reference: string, description: string, category: string, rawValue: string | null, normalizedValue: string | null, unit: string | null, sourceScope: string, capturedAt: string | null, expiresAt: string | null, isCached: boolean, collectionMethod: string | null }
- missingContextItems: array of { reference: string, description: string, reason: string, sourceScope: string, severity: string }
- collectionMetadata: array of { name: string, value: string, description: string | null }
- warnings: array of strings
不要输出 Markdown，不要输出解释性前后缀，不要输出代码块。
"""
        };
    }

    private static bool ShouldUseJsonResponseFormat(DbConfigDiagnosisAgentOptions options)
    {
        return options.Model.Contains("qwen", StringComparison.OrdinalIgnoreCase)
            || options.Model.Contains("deepseek", StringComparison.OrdinalIgnoreCase)
            || options.Endpoint.Contains("dashscope", StringComparison.OrdinalIgnoreCase)
            || options.Endpoint.Contains("aliyuncs", StringComparison.OrdinalIgnoreCase);
    }

    private static void EnforceNoSpecificTargetValuesWithoutHostContext(
        DbConfigEvidencePack evidence,
        DbConfigDiagnosisAgentResponse response)
    {
        if (evidence.HostContextItems.Count > 0)
        {
            return;
        }

        foreach (var recommendation in response.Recommendations)
        {
            if (recommendation.RecommendationClass is not ("tuning" or "capacity-planning"))
            {
                continue;
            }

            var text = $"{recommendation.Suggestion} {recommendation.AppliesWhen} {recommendation.ImpactSummary}";
            if (ContainsSpecificTargetValue(text))
            {
                throw new InvalidOperationException("缺少宿主上下文时，智能体不能返回具体目标参数值。");
            }
        }
    }

    private static bool ContainsSpecificTargetValue(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        return System.Text.RegularExpressions.Regex.IsMatch(
            text,
            @"(\d+(\.\d+)?\s*(KB|MB|GB|TB|%|cores?))|(调整到|设置为|set to|tune to)",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
    }
}

public sealed record DbConfigDiagnosisAgentResponse(
    string Title,
    string Summary,
    IReadOnlyList<DbConfigRecommendation> Recommendations,
    IReadOnlyList<DbConfigEvidenceItem> EvidenceItems,
    IReadOnlyList<DbConfigMissingContextItem> MissingContextItems,
    IReadOnlyList<DbConfigCollectionMetadataItem> CollectionMetadata,
    IReadOnlyList<string> Warnings);

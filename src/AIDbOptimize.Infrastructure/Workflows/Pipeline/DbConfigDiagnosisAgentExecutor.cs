using System.ClientModel;
using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Security;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Db config diagnosis agent executor.
/// </summary>
public sealed class DbConfigDiagnosisAgentExecutor(
    DbConfigDiagnosisAgentOptions options,
    DiagnosisRuleEvaluator diagnosisRuleEvaluator,
    ILoggerFactory loggerFactory,
    IServiceProvider services)
    : IDbConfigDiagnosisAgentExecutor
{
    private readonly DbConfigDiagnosisAgentOptions _options = options;
    private readonly DiagnosisRuleEvaluator _diagnosisRuleEvaluator = diagnosisRuleEvaluator;
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
        DbConfigEvidencePack evidence,
        DiagnosisSkillDefinition? diagnosisSkill = null)
    {
        return PromptInputBuilder.BuildDbConfigPrompt(
            displayName,
            databaseName,
            engine,
            optimizationGoal,
            notes: null,
            evidence,
            diagnosisSkill);
    }

    public async Task<DbConfigDiagnosisExecutionResult> ExecuteAsync(
        DbConfigEvidencePack evidence,
        string? optimizationGoal,
        DiagnosisSkillDefinition? diagnosisSkill = null,
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
            evidence: evidence,
            diagnosisSkill: diagnosisSkill);

        var agent = BuildAgent(_options, _loggerFactory, _services);
        // 让 ChatClientAgent 在每次调用时自行创建会话，避免依赖服务端会话存储。
        var response = await agent.RunAsync(
            prompt,
            session: null,
            cancellationToken: cancellationToken);

        var responseText = response.Text?.Trim();
        if (string.IsNullOrWhiteSpace(responseText))
        {
            throw new InvalidOperationException("MAF 诊断智能体没有返回内容。");
        }

        var value = JsonSerializer.Deserialize<DbConfigDiagnosisAgentResponse>(responseText, SerializerOptions)
            ?? throw new InvalidOperationException("MAF 诊断智能体返回的 JSON 无法解析。");
        EnforceNoSpecificTargetValuesWithoutHostContext(evidence, value);

        var recommendations = diagnosisSkill is null
            ? value.Recommendations.Select(EnsureRecommendationType).ToArray()
            : _diagnosisRuleEvaluator.Evaluate(evidence, diagnosisSkill, value.Recommendations);

        var reportJson = JsonSerializer.Serialize(new
        {
            title = value.Title,
            summary = value.Summary,
            recommendations,
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
            throw new InvalidOperationException(
                "DbConfig 诊断智能体未配置有效 ApiKey。请先在 src/AIDbOptimize.ApiService/appsettings.json 的占位配置基础上填写真实值，" +
                "并优先在 src/AIDbOptimize.ApiService/appsettings.Local.json 的 AIDbOptimize:Agent:Diagnosis:ApiKey 中覆盖。");
        }

        var client = new ChatClient(
            model: options.Model,
            credential: new ApiKeyCredential(options.ApiKey),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri(options.Endpoint),
                NetworkTimeout = TimeSpan.FromMinutes(3)
            });

        return client.AsAIAgent(
            options: new ChatClientAgentOptions
            {
                Name = "DbConfigDiagnosisAgent",
                Description = "Generate a grounded db configuration optimization report.",
                ChatOptions = BuildChatOptions(options)
            },
            clientFactory: null,
            loggerFactory: loggerFactory,
            services: services);
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
只允许基于输入证据推理，不允许补造环境信息或观测数据。
只输出一个合法 JSON 对象，不要输出 Markdown、解释性前后缀或代码块。
顶层字段固定为 title、summary、recommendations、evidenceItems、missingContextItems、collectionMetadata、warnings。
recommendations 中每一项都必须包含 key、suggestion、severity、findingType、confidence、requiresMoreContext、impactSummary、evidenceReferences、recommendationClass、recommendationType、appliesWhen、ruleId、ruleVersion。
evidenceItems 中每一项都必须包含 sourceType、reference、description、category、rawValue、normalizedValue、unit、sourceScope、capturedAt、expiresAt、isCached、collectionMethod。
missingContextItems 中每一项都必须包含 reference、description、reason、sourceScope、severity。
collectionMetadata 中每一项都必须包含 name、value、description。
recommendationType 只能是 actionableRecommendation 或 requestMoreContext。
evidenceItems、missingContextItems、collectionMetadata 的字段名必须与输入 contract 保持一致。
title、summary、suggestion 等自然语言字段必须使用中文。
如果缺少宿主资源上下文，不允许给出具体目标参数值或“调到 X GB / X MB / X%”这类结论，只能给出保守建议或 requestMoreContext。
当 Host Context Availability=missing 时，凡是需要具体数值目标的 tuning / capacity recommendation，都必须改为 recommendationType=requestMoreContext；suggestion、appliesWhen、impactSummary 中禁止出现数字+单位以及“设置为 / 调整到 / set to / tune to”这类表达。
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

    private static DbConfigRecommendation EnsureRecommendationType(DbConfigRecommendation recommendation)
    {
        var recommendationType = recommendation.RequiresMoreContext
            ? DbConfigRecommendationType.RequestMoreContext
            : recommendation.RecommendationType;

        return new DbConfigRecommendation(
            recommendation.Key,
            recommendation.Suggestion,
            recommendation.Severity,
            recommendation.FindingType,
            recommendation.Confidence,
            recommendation.RequiresMoreContext,
            recommendation.ImpactSummary,
            recommendation.EvidenceReferences,
            recommendation.RecommendationClass,
            recommendationType,
            recommendation.AppliesWhen,
            recommendation.RuleId,
            recommendation.RuleVersion);
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

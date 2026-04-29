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
        var evidenceJson = JsonSerializer.Serialize(evidence, SerializerOptions);
        return PromptInputBuilder.BuildDbConfigPrompt(
            displayName,
            databaseName,
            engine,
            optimizationGoal,
            notes: null,
            evidenceJson);
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
            throw new InvalidOperationException("MAF diagnosis agent returned empty text.");
        }

        var value = JsonSerializer.Deserialize<DbConfigDiagnosisAgentResponse>(responseText, SerializerOptions)
            ?? throw new InvalidOperationException("MAF diagnosis agent returned invalid JSON.");

        var reportJson = JsonSerializer.Serialize(new
        {
            title = value.Title,
            summary = value.Summary,
            recommendations = value.Recommendations,
            evidenceItems = value.EvidenceItems,
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
            throw new InvalidOperationException("DbConfig diagnosis agent is not configured.");
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
You are a database configuration optimization assistant.
Use only the provided evidence.
Return JSON only.
The JSON object must contain:
- title: string
- summary: string
- recommendations: array of { key: string, suggestion: string, severity: string }
- evidenceItems: array of { sourceType: string, reference: string, description: string }
- warnings: array of strings
Do not add markdown, explanations, or code fences.
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
}

public sealed record DbConfigDiagnosisAgentResponse(
    string Title,
    string Summary,
    IReadOnlyList<DbConfigRecommendation> Recommendations,
    IReadOnlyList<DbConfigEvidenceItem> EvidenceItems,
    IReadOnlyList<string> Warnings);

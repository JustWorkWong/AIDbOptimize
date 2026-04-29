using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Db config diagnosis agent abstraction.
/// </summary>
public interface IDbConfigDiagnosisAgentExecutor
{
    string PromptVersion { get; }

    string ModelId { get; }

    string BuildPrompt(
        string displayName,
        string databaseName,
        string engine,
        string optimizationGoal,
        DbConfigEvidencePack evidence);

    Task<DbConfigDiagnosisExecutionResult> ExecuteAsync(
        DbConfigEvidencePack evidence,
        string? optimizationGoal,
        CancellationToken cancellationToken = default);
}

public sealed record DbConfigDiagnosisExecutionResult(
    string ReportJson,
    string TokenUsageJson);

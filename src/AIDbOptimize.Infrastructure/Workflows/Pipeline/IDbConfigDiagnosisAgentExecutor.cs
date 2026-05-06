using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Workflows.Skills;

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
        DbConfigEvidencePack evidence,
        DiagnosisSkillDefinition? diagnosisSkill = null);

    Task<DbConfigDiagnosisExecutionResult> ExecuteAsync(
        DbConfigEvidencePack evidence,
        string? optimizationGoal,
        DiagnosisSkillDefinition? diagnosisSkill = null,
        CancellationToken cancellationToken = default);
}

public sealed record DbConfigDiagnosisExecutionResult(
    string ReportJson,
    string TokenUsageJson);

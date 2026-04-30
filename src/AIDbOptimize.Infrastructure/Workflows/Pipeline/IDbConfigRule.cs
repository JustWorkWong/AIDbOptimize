using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public interface IDbConfigRule
{
    string RuleId { get; }

    string RuleVersion { get; }

    DatabaseEngine Engine { get; }

    ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(
        DbConfigCollectedContext context,
        CancellationToken cancellationToken = default);
}

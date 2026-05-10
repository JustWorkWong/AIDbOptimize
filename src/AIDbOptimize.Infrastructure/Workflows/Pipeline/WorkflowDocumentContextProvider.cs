using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Workflows.Skills;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed class WorkflowDocumentContextProvider(IRagKnowledgeQueryService ragKnowledgeQueryService)
{
    public Task<RagKnowledgeQueryResult> GetContextAsync(
        InvestigationPlan plan,
        DbConfigEvidencePack evidence,
        CancellationToken cancellationToken = default)
    {
        return ragKnowledgeQueryService.QueryAsync(plan, evidence, cancellationToken);
    }
}

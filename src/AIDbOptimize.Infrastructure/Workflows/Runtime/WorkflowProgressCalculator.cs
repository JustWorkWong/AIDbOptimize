namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

internal static class WorkflowProgressCalculator
{
    private static readonly IReadOnlyDictionary<string, int> ProgressByNode = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["DbConfigInputValidationExecutor"] = 10,
        ["DbConfigSnapshotCollectorExecutor"] = 25,
        ["DbConfigRuleAnalysisExecutor"] = 45,
        ["DbConfigDiagnosisAgentExecutor"] = 65,
        ["DbConfigGroundingExecutor"] = 80,
        ["DbConfigHumanReviewGateExecutor"] = 90,
        ["DbConfigCompletionExecutor"] = 100,
        ["Cancelled"] = 100
    };

    public static int GetProgressPercent(string? currentNode, string status)
    {
        if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            return 100;
        }

        if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
        {
            return 100;
        }

        return currentNode is not null && ProgressByNode.TryGetValue(currentNode, out var progress)
            ? progress
            : 0;
    }
}

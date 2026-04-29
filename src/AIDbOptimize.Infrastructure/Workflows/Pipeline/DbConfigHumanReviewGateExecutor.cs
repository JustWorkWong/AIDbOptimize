namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 人工审核 gate 执行器。
/// </summary>
public sealed class DbConfigHumanReviewGateExecutor
{
    public HumanReviewGateResult Decide(bool requireHumanReview, string reportTitle)
    {
        if (!requireHumanReview)
        {
            return new HumanReviewGateResult(HumanReviewDecision.CompleteDirectly, null);
        }

        return new HumanReviewGateResult(
            HumanReviewDecision.RequireReview,
            $"审核 {reportTitle}");
    }
}

/// <summary>
/// 人工审核决策。
/// </summary>
public enum HumanReviewDecision
{
    CompleteDirectly = 1,
    RequireReview = 2
}

/// <summary>
/// 人工审核 gate 决策结果。
/// </summary>
public sealed record HumanReviewGateResult(
    HumanReviewDecision Decision,
    string? Title);

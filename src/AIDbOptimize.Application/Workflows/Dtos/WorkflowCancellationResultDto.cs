namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// workflow 取消结果。
/// </summary>
public sealed record WorkflowCancellationResultDto(
    string SessionId,
    bool Accepted,
    string Status);

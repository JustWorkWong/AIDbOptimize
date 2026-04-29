using System.Text.Json;

namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Submit review request.
/// </summary>
public sealed record SubmitReviewRequest(
    string Action,
    string? Reviewer,
    string? Comment,
    JsonElement? Adjustments = null);

using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Application.Workflows.Services;

namespace AIDbOptimize.ApiService.Api;

/// <summary>
/// review API。
/// </summary>
internal static class ReviewsApiRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapReviewsApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/reviews");

        group.MapGet(string.Empty, HandleListReviewsAsync);
        group.MapGet("/{taskId}", HandleGetReviewAsync);
        group.MapPost("/{taskId}/submit", HandleSubmitReviewAsync);

        return endpoints;
    }

    private static async Task<IResult> HandleListReviewsAsync(
        IWorkflowReviewService reviewService,
        CancellationToken cancellationToken)
    {
        var result = await reviewService.ListAsync(cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> HandleGetReviewAsync(
        string taskId,
        IWorkflowReviewService reviewService,
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetAsync(taskId, cancellationToken);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> HandleSubmitReviewAsync(
        string taskId,
        SubmitReviewRequest request,
        IWorkflowReviewService reviewService,
        CancellationToken cancellationToken)
    {
        var result = await reviewService.SubmitAsync(taskId, request, cancellationToken);
        return Results.Ok(result);
    }
}

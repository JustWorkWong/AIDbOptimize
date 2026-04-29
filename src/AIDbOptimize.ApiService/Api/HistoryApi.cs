using AIDbOptimize.Application.Workflows.Services;

namespace AIDbOptimize.ApiService.Api;

/// <summary>
/// workflow 历史 API。
/// </summary>
internal static class HistoryApiRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapHistoryApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/history");

        group.MapGet(string.Empty, HandleListHistoryAsync);
        group.MapGet("/{sessionId}", HandleGetHistoryAsync);

        return endpoints;
    }

    private static async Task<IResult> HandleListHistoryAsync(
        IWorkflowHistoryService historyService,
        CancellationToken cancellationToken)
    {
        var result = await historyService.ListAsync(cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> HandleGetHistoryAsync(
        string sessionId,
        IWorkflowHistoryService historyService,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetAsync(sessionId, cancellationToken);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}

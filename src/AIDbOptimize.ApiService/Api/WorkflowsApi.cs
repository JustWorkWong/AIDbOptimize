using AIDbOptimize.Application.Workflows.Services;

namespace AIDbOptimize.ApiService.Api;

/// <summary>
/// Workflow API.
/// </summary>
internal static class WorkflowsApiRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapWorkflowsApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/workflows");

        group.MapPost("/db-config-optimization", HandleStartDbConfigOptimizationAsync);
        group.MapGet("/{sessionId}", HandleGetSessionAsync);
        group.MapGet(string.Empty, HandleListSessionsAsync);
        group.MapPost("/{sessionId}/cancel", HandleCancelAsync);

        return endpoints;
    }

    private static async Task<IResult> HandleStartDbConfigOptimizationAsync(
        Application.Workflows.Dtos.StartDbConfigOptimizationWorkflowRequest request,
        IDbConfigOptimizationWorkflowService workflowService,
        CancellationToken cancellationToken)
    {
        var result = await workflowService.StartAsync(request, cancellationToken);
        return Results.Accepted($"/api/workflows/{result.SessionId}", result);
    }

    private static async Task<IResult> HandleGetSessionAsync(
        string sessionId,
        IDbConfigOptimizationWorkflowService workflowService,
        CancellationToken cancellationToken)
    {
        var result = await workflowService.GetSessionAsync(sessionId, cancellationToken);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> HandleListSessionsAsync(
        IDbConfigOptimizationWorkflowService workflowService,
        CancellationToken cancellationToken)
    {
        var result = await workflowService.ListSessionsAsync(cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> HandleCancelAsync(
        string sessionId,
        IDbConfigOptimizationWorkflowService workflowService,
        CancellationToken cancellationToken)
    {
        var result = await workflowService.CancelAsync(sessionId, cancellationToken);
        return Results.Accepted($"/api/workflows/{sessionId}", result);
    }
}

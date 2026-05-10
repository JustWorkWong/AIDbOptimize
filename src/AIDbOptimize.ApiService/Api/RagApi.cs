using AIDbOptimize.Application.Rag.Services;

namespace AIDbOptimize.ApiService.Api;

internal static class RagApiRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapRagApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/rag/assets/status", HandleGetAssetStatusAsync);
        endpoints.MapGet("/api/rag/cases/audit", HandleGetCaseAuditAsync);
        endpoints.MapPost("/api/rag/validate", HandleValidateAsync);
        endpoints.MapPost("/api/rag/rebuild", HandleRebuildAsync);
        return endpoints;
    }

    private static async Task<IResult> HandleGetAssetStatusAsync(
        RagAssetStatusAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.GetAsync(cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> HandleGetCaseAuditAsync(
        RagCaseAuditAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.ListAsync(cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> HandleValidateAsync(
        RagOperationsRequest? request,
        RagOperationsAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.ValidateAsync(request?.CorpusRootPath, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> HandleRebuildAsync(
        RagOperationsRequest? request,
        RagOperationsAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.RebuildAsync(request?.CorpusRootPath, cancellationToken);
        return Results.Ok(result);
    }
}

internal sealed record RagOperationsRequest(string? CorpusRootPath);

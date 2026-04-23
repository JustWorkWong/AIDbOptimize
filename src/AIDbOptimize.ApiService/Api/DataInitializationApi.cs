using AIDbOptimize.Application.DataInitialization.Services;

namespace AIDbOptimize.ApiService.Api;

/// <summary>
/// 数据初始化状态 API。
/// 用于前端查看 PostgreSQL / MySQL 业务测试库初始化进度。
/// </summary>
internal static class DataInitializationApiRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapDataInitializationApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/data-init/status", HandleGetStatusAsync);
        return endpoints;
    }

    private static async Task<IResult> HandleGetStatusAsync(
        InitializationStatusAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.GetLatestAsync(cancellationToken);
        return Results.Ok(result);
    }
}

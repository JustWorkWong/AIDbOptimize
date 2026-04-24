using ModelContextProtocol.Client;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP 进程会话包装。
/// 用于在一次发现或执行操作期间持有客户端实例，并在结束时统一释放。
/// </summary>
public sealed class McpProcessSession(McpClient client) : IAsyncDisposable
{
    /// <summary>
    /// 已连接的 MCP 客户端。
    /// </summary>
    public McpClient Client { get; } = client;

    public async ValueTask DisposeAsync()
    {
        await Client.DisposeAsync();
    }
}

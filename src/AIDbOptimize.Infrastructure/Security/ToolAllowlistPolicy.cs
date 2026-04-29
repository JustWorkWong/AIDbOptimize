namespace AIDbOptimize.Infrastructure.Security;

/// <summary>
/// workflow 工具白名单策略。
/// </summary>
public static class ToolAllowlistPolicy
{
    private static readonly HashSet<string> ReadOnlyToolNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "query",
        "execute_query",
        "describe_table",
        "show_indexes",
        "get_config",
        "explain",
        "resolve_runtime_target",
        "get_container_limits",
        "get_container_stats",
        "get_disk_usage",
        "get_host_memory",
        "get_host_cpu",
        "get_process_limits",
        "get_managed_service_profile"
    };

    public static bool IsAllowed(string? toolName, bool isWriteTool)
    {
        if (isWriteTool)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(toolName))
        {
            return false;
        }

        return ReadOnlyToolNames.Contains(toolName);
    }
}

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
        "explain"
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

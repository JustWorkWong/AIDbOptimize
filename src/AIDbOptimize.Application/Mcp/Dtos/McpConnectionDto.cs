using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;
using System.Text.RegularExpressions;

namespace AIDbOptimize.Application.Mcp.Dtos;

/// <summary>
/// MCP connection query result.
/// </summary>
public sealed record McpConnectionDto(
    Guid Id,
    string Name,
    DatabaseEngine Engine,
    string DisplayName,
    string DatabaseName,
    string CommandPreview,
    string CommandLine,
    IReadOnlyList<string> EnvironmentEntries,
    bool IsDefault,
    McpConnectionStatus Status,
    DateTimeOffset? LastDiscoveredAt)
{
    public static McpConnectionDto FromDefinition(McpConnectionDefinition definition)
    {
        var environmentEntries = definition.EnvironmentVariables
            .Select(pair => $"{pair.Key}={Quote(MaskFreeText(pair.Value))}")
            .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var commandLine = string.Join(
            " ",
            new[] { definition.ServerCommand }
                .Concat(definition.ServerArguments.Select(argument => Quote(MaskFreeText(argument)))));

        var commandPreview = environmentEntries.Length == 0
            ? commandLine
            : string.Join(" ", environmentEntries.Concat([commandLine]));

        return new McpConnectionDto(
            definition.Id,
            definition.Name,
            definition.Engine,
            definition.DisplayName,
            definition.DatabaseName,
            commandPreview,
            commandLine,
            environmentEntries,
            definition.IsDefault,
            definition.Status,
            definition.LastDiscoveredAt);
    }

    private static string Quote(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "\"\"";
        }

        return value.Any(char.IsWhiteSpace) || value.Contains('"')
            ? $"\"{value.Replace("\"", "\\\"", StringComparison.Ordinal)}\""
            : value;
    }

    private static string MaskFreeText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var masked = Regex.Replace(
            value,
            "(?i)(password|pwd)=([^;\\s]+)",
            "$1=***");
        masked = Regex.Replace(
            masked,
            "(?i)(api[_-]?key|secret|token)=([^;\\s]+)",
            "$1=***");
        masked = Regex.Replace(
            masked,
            "(?i)(://[^:]+:)([^@]+)(@)",
            "$1***$3");
        return masked;
    }
}

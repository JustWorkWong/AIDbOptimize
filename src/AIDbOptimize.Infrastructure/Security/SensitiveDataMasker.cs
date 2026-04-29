using System.Text.RegularExpressions;

namespace AIDbOptimize.Infrastructure.Security;

/// <summary>
/// 敏感数据脱敏工具。
/// </summary>
public static class SensitiveDataMasker
{
    public static string MaskConnectionString(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var masked = Regex.Replace(
            value,
            "(?i)(password|pwd)=([^;]+)",
            "$1=***");

        masked = Regex.Replace(
            masked,
            "(?i)(user id|uid|username)=([^;]+)",
            "$1=***");

        return masked;
    }

    public static string MaskFreeText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var masked = MaskConnectionString(value);
        masked = Regex.Replace(masked, "(?i)(password\"?\\s*[:=]\\s*\")([^\"]+)(\")", "$1***$3");
        masked = Regex.Replace(masked, "(?i)(api[_-]?key\"?\\s*[:=]\\s*\")([^\"]+)(\")", "$1***$3");
        masked = Regex.Replace(masked, "(?i)(secret\"?\\s*[:=]\\s*\")([^\"]+)(\")", "$1***$3");
        masked = Regex.Replace(masked, "(?i)(token\"?\\s*[:=]\\s*\")([^\"]+)(\")", "$1***$3");
        return masked;
    }
}

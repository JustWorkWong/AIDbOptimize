namespace AIDbOptimize.Infrastructure.Security;

/// <summary>
/// 工具输出脱敏器。
/// </summary>
public static class ToolOutputSanitizer
{
    public static string SanitizeJson(string? rawJson)
    {
        return SensitiveDataMasker.MaskFreeText(rawJson);
    }
}

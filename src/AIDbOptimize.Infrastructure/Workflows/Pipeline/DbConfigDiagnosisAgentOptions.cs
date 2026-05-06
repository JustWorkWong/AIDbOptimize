using System;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 数据库配置优化诊断 agent 配置。
/// </summary>
public sealed class DbConfigDiagnosisAgentOptions
{
    public string PromptVersion { get; init; } = "db-config-diagnosis-v1";

    public string Endpoint { get; init; } = "https://dashscope.aliyuncs.com/compatible-mode/v1";

    public string Model { get; init; } = "qwen3.6-plus";

    public string ApiKey { get; init; } = string.Empty;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey) && !IsPlaceholder(ApiKey);

    private static bool IsPlaceholder(string apiKey)
    {
        return string.Equals(apiKey, "xxx", StringComparison.OrdinalIgnoreCase)
            || string.Equals(apiKey, "YOUR_DASHSCOPE_API_KEY", StringComparison.OrdinalIgnoreCase)
            || string.Equals(apiKey, "YOUR_API_KEY", StringComparison.OrdinalIgnoreCase);
    }
}

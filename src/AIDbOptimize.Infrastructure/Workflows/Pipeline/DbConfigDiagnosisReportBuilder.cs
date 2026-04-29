using System.Text;
using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 数据库配置优化报告构造器。
/// 当前先用确定性方式输出人类可读报告。
/// </summary>
public sealed class DbConfigDiagnosisReportBuilder
{
    public string Build(DbConfigEvidencePack evidence, string? optimizationGoal)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"数据库：{evidence.DatabaseName}");
        builder.AppendLine($"来源：{evidence.Source}");

        if (!string.IsNullOrWhiteSpace(optimizationGoal))
        {
            builder.AppendLine($"目标：{optimizationGoal}");
        }

        builder.AppendLine("建议：");
        foreach (var recommendation in evidence.Recommendations)
        {
            builder.AppendLine($"- {recommendation.Key}: {recommendation.Suggestion} ({recommendation.Severity})");
        }

        if (evidence.Warnings.Count > 0)
        {
            builder.AppendLine("警告：");
            foreach (var warning in evidence.Warnings)
            {
                builder.AppendLine($"- {warning}");
            }
        }

        return builder.ToString().Trim();
    }
}

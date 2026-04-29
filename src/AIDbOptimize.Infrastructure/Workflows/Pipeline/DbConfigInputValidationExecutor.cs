using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 数据库配置优化输入校验执行器。
/// </summary>
public sealed class DbConfigInputValidationExecutor
{
    public void Validate(
        Guid connectionId,
        string databaseName,
        DatabaseEngine engine,
        bool requireHumanReview)
    {
        if (connectionId == Guid.Empty)
        {
            throw new InvalidOperationException("connectionId 不能为空。");
        }

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("databaseName 不能为空。");
        }

        if (engine is not (DatabaseEngine.PostgreSql or DatabaseEngine.MySql))
        {
            throw new InvalidOperationException("当前仅支持 PostgreSql / MySql。");
        }

        _ = requireHumanReview;
    }
}

namespace AIDbOptimize.Domain.Common;

/// <summary>
/// 领域层统一异常。
/// 用于表达业务规则被破坏，而不是技术错误。
/// </summary>
public sealed class DomainException(string message) : Exception(message);

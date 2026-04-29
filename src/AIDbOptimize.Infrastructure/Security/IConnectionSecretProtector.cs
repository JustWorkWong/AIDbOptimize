namespace AIDbOptimize.Infrastructure.Security;

public interface IConnectionSecretProtector
{
    string ProtectIfNeeded(string? plaintext);

    string UnprotectIfNeeded(string? protectedText);
}

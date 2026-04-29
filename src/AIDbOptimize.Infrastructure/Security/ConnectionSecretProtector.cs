using Microsoft.AspNetCore.DataProtection;

namespace AIDbOptimize.Infrastructure.Security;

public sealed class ConnectionSecretProtector(IDataProtectionProvider dataProtectionProvider)
    : IConnectionSecretProtector
{
    private const string Prefix = "enc::";
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("AIDbOptimize.McpConnectionSecrets.v1");

    public string ProtectIfNeeded(string? plaintext)
    {
        if (string.IsNullOrWhiteSpace(plaintext))
        {
            return string.Empty;
        }

        if (plaintext.StartsWith(Prefix, StringComparison.Ordinal))
        {
            return plaintext;
        }

        return Prefix + _protector.Protect(plaintext);
    }

    public string UnprotectIfNeeded(string? protectedText)
    {
        if (string.IsNullOrWhiteSpace(protectedText))
        {
            return string.Empty;
        }

        if (!protectedText.StartsWith(Prefix, StringComparison.Ordinal))
        {
            return protectedText;
        }

        var payload = protectedText[Prefix.Length..];
        return _protector.Unprotect(payload);
    }
}

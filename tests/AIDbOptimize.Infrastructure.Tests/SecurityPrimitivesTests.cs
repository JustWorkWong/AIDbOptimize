using AIDbOptimize.Infrastructure.Security;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Persistence.Repositories;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class SecurityPrimitivesTests
{
    [Fact]
    public void SensitiveDataMasker_MasksPasswordInConnectionString()
    {
        var input = "Host=localhost;Port=5432;Username=postgres;Password=Secret123!;Database=appdb";

        var masked = SensitiveDataMasker.MaskConnectionString(input);

        Assert.DoesNotContain("Secret123!", masked, StringComparison.Ordinal);
        Assert.Contains("Password=***", masked, StringComparison.Ordinal);
    }

    [Fact]
    public void ToolOutputSanitizer_MasksSensitiveFieldsInJson()
    {
        var input = """{"password":"Secret123!","apiKey":"abc","host":"localhost"}""";

        var sanitized = ToolOutputSanitizer.SanitizeJson(input);

        Assert.DoesNotContain("Secret123!", sanitized, StringComparison.Ordinal);
        Assert.DoesNotContain(@"""apiKey"":""abc""", sanitized, StringComparison.Ordinal);
        Assert.Contains("***", sanitized, StringComparison.Ordinal);
    }

    [Fact]
    public void ToolAllowlistPolicy_RejectsWriteTool()
    {
        var allowed = ToolAllowlistPolicy.IsAllowed("execute_write_sql", isWriteTool: true);

        Assert.False(allowed);
    }

    [Fact]
    public void ToolAllowlistPolicy_AllowsReadTool()
    {
        var allowed = ToolAllowlistPolicy.IsAllowed("query", isWriteTool: false);

        Assert.True(allowed);
    }

    [Fact]
    public void PromptInputBuilder_BuildsMaskedPromptContext()
    {
        var context = PromptInputBuilder.BuildDbConfigPrompt(
            displayName: "postgres-main",
            databaseName: "appdb",
            engine: "PostgreSql",
            optimizationGoal: "降低慢查询",
            notes: "连接串 Host=localhost;Password=Secret123!",
            evidenceJson: """{"shared_buffers":"256MB"}""");

        Assert.Contains("postgres-main", context, StringComparison.Ordinal);
        Assert.Contains("降低慢查询", context, StringComparison.Ordinal);
        Assert.DoesNotContain("Secret123!", context, StringComparison.Ordinal);
        Assert.Contains("Password=***", context, StringComparison.Ordinal);
    }

    [Fact]
    public async Task McpConnectionRepository_EncryptsSecretsAtRest_AndDecryptsOnRead()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"mcp-connection-protection-{Guid.NewGuid():N}")
            .Options;
        await using var dbContext = new ControlPlaneDbContext(options);
        var factory = new TestDbContextFactory(options);
        var protector = new ConnectionSecretProtector(new FakeDataProtectionProvider());
        var repository = new McpConnectionRepository(factory, protector);
        var now = DateTimeOffset.UtcNow;

        var record = new McpConnectionRecord(
            Guid.NewGuid(),
            "secure-conn",
            DatabaseEngine.PostgreSql,
            "Secure PostgreSQL",
            "npx",
            """["-y","@modelcontextprotocol/server-postgres","postgresql://postgres:Secret123!@localhost:15432/appdb"]""",
            """{"PGPASSWORD":"Secret123!"}""",
            "Host=localhost;Port=15432;Username=postgres;Password=Secret123!;Database=appdb",
            "appdb",
            false,
            McpConnectionStatus.Ready,
            null,
            now,
            now);

        await repository.AddAsync(record);

        var stored = await dbContext.McpConnections.SingleAsync(x => x.Id == record.Id);
        Assert.StartsWith("enc::", stored.ServerArgumentsJson, StringComparison.Ordinal);
        Assert.StartsWith("enc::", stored.EnvironmentJson, StringComparison.Ordinal);
        Assert.StartsWith("enc::", stored.DatabaseConnectionString, StringComparison.Ordinal);
        Assert.DoesNotContain("Secret123!", stored.ServerArgumentsJson, StringComparison.Ordinal);
        Assert.DoesNotContain("Secret123!", stored.EnvironmentJson, StringComparison.Ordinal);
        Assert.DoesNotContain("Secret123!", stored.DatabaseConnectionString, StringComparison.Ordinal);

        var roundTripped = await repository.GetByIdAsync(record.Id);
        Assert.NotNull(roundTripped);
        Assert.Contains("Secret123!", roundTripped!.ServerArgumentsJson, StringComparison.Ordinal);
        Assert.Contains("Secret123!", roundTripped.EnvironmentJson, StringComparison.Ordinal);
        Assert.Contains("Secret123!", roundTripped.DatabaseConnectionString, StringComparison.Ordinal);
    }

    private sealed class TestDbContextFactory(DbContextOptions<ControlPlaneDbContext> options)
        : IDbContextFactory<ControlPlaneDbContext>
    {
        public ControlPlaneDbContext CreateDbContext()
        {
            return new ControlPlaneDbContext(options);
        }

        public Task<ControlPlaneDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateDbContext());
        }
    }

    private sealed class FakeDataProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return new FakeDataProtector();
        }
    }

    private sealed class FakeDataProtector : IDataProtector
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return this;
        }

        public byte[] Protect(byte[] plaintext)
        {
            return Encoding.UTF8.GetBytes(Convert.ToBase64String(plaintext));
        }

        public string Protect(string plaintext)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plaintext));
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return Convert.FromBase64String(Encoding.UTF8.GetString(protectedData));
        }

        public string Unprotect(string protectedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(protectedData));
        }
    }
}

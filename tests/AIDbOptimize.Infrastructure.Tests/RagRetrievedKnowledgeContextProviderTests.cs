using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using Microsoft.Extensions.AI;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagRetrievedKnowledgeContextProviderTests
{
    [Fact]
    public async Task ProvideMessagesAsync_ReturnsSystemMessageWhenExternalKnowledgeExists()
    {
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            [],
            [],
            externalKnowledgeItems:
            [
                new DbConfigEvidenceItem(
                    "fact",
                    "[fact:mysql:memory] innodb_buffer_pool_size > Memory | https://example.com/mysql",
                    "MySQL reference",
                    "externalKnowledge",
                    "innodb_buffer_pool_size controls InnoDB cache sizing.",
                    "citation")
            ]);
        var provider = new TestableRagRetrievedKnowledgeContextProvider(evidence);

        var messages = (await provider.InvokeAsync()).ToArray();

        var message = Assert.Single(messages);
        Assert.Equal(ChatRole.System, message.Role);
    }

    [Fact]
    public async Task ProvideMessagesAsync_ReturnsEmptyWhenNoExternalKnowledgeExists()
    {
        var evidence = new DbConfigEvidencePack(DatabaseEngine.PostgreSql, "appdb", "collector", [], [], []);
        var provider = new TestableRagRetrievedKnowledgeContextProvider(evidence);

        var messages = await provider.InvokeAsync();

        Assert.Empty(messages);
    }

    private sealed class TestableRagRetrievedKnowledgeContextProvider(DbConfigEvidencePack evidence)
        : RagRetrievedKnowledgeContextProvider(evidence)
    {
        public ValueTask<IEnumerable<ChatMessage>> InvokeAsync()
        {
            return ProvideMessagesAsync(null!, CancellationToken.None);
        }
    }
}

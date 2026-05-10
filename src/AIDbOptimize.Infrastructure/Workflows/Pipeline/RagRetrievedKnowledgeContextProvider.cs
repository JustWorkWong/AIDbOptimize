using System.Text;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public class RagRetrievedKnowledgeContextProvider(
    DbConfigEvidencePack evidence) : MessageAIContextProvider
{
    protected override ValueTask<IEnumerable<ChatMessage>> ProvideMessagesAsync(
        InvokingContext context,
        CancellationToken cancellationToken)
    {
        if (evidence.ExternalKnowledgeItems.Count == 0)
        {
            return ValueTask.FromResult<IEnumerable<ChatMessage>>([]);
        }

        var builder = new StringBuilder();
        builder.AppendLine("以下是工作流在诊断前检索到的外部知识，仅可作为 supplemental context 使用，不可覆盖 observed evidence 或缺失上下文约束。");
        foreach (var item in evidence.ExternalKnowledgeItems)
        {
            builder.Append("- 引用: ").AppendLine(item.Reference);
            builder.Append("  分类: ").AppendLine(item.Category);
            builder.Append("  摘要: ").AppendLine(item.Description);
            if (!string.IsNullOrWhiteSpace(item.RawValue))
            {
                builder.Append("  片段: ").AppendLine(item.RawValue);
            }

            if (!string.IsNullOrWhiteSpace(item.NormalizedValue))
            {
                builder.Append("  Citation: ").AppendLine(item.NormalizedValue);
            }
        }

        IEnumerable<ChatMessage> messages = [new(ChatRole.System, builder.ToString())];
        return ValueTask.FromResult(messages);
    }
}

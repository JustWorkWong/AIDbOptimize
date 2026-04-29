using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Agents;

/// <summary>
/// Agent rolling summary 服务实现。
/// </summary>
public sealed class AgentSummaryService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IAgentSummaryService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<AgentSummaryResult> CreateRollingSummaryAsync(
        AgentSummaryCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        using var activity = AIDbOptimizeTelemetry.AgentActivitySource.StartActivity("agent.summary.create");
        activity?.SetTag("agent.session_id", request.AgentSessionId);
        activity?.SetTag("agent.summary_type", request.SummaryType);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = new AgentSummaryEntity
        {
            Id = Guid.NewGuid(),
            AgentSessionId = request.AgentSessionId,
            SummaryType = request.SummaryType,
            SummaryJson = request.SummaryJson,
            SourceStartSequence = request.SourceStartSequence,
            SourceEndSequence = request.SourceEndSequence,
            CreatedAt = request.CreatedAt
        };

        dbContext.AgentSummaries.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        AIDbOptimizeTelemetry.AgentSummaryGenerated.Add(1);

        return new AgentSummaryResult(entity.Id, entity.SummaryJson);
    }

    public static string BuildRollingSummaryJson(
        string workflowType,
        string databaseName,
        string report,
        string? optimizationGoal,
        int messageCount)
    {
        return JsonSerializer.Serialize(new
        {
            workflowType,
            databaseName,
            optimizationGoal,
            messageCount,
            summary = report.Length > 400 ? report[..400] : report
        }, SerializerOptions);
    }
}

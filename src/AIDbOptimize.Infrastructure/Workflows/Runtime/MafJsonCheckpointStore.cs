using System.Text.Json;
using System.Text.Json.Nodes;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Checkpointing;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

public sealed class MafJsonCheckpointStore(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : ICheckpointStore<JsonElement>
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async ValueTask<IEnumerable<CheckpointInfo>> RetrieveIndexAsync(
        string sessionId,
        CheckpointInfo? withParent)
    {
        if (!Guid.TryParse(sessionId, out var workflowSessionId))
        {
            return [];
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var query = dbContext.WorkflowCheckpoints
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.Sequence);

        return await query
            .Select(x => new CheckpointInfo(sessionId, x.CheckpointRef))
            .ToListAsync();
    }

    public async ValueTask<CheckpointInfo> CreateCheckpointAsync(
        string sessionId,
        JsonElement value,
        CheckpointInfo? parent = null)
    {
        if (!Guid.TryParse(sessionId, out var workflowSessionId))
        {
            throw new InvalidOperationException($"Invalid workflow session id: {sessionId}");
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == workflowSessionId);

        var nextSequence = (await dbContext.WorkflowCheckpoints
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .Select(x => (int?)x.Sequence)
            .MaxAsync() ?? 0) + 1;

        var snapshotJson = EnrichSnapshotJson(value.GetRawText(), session.InputPayloadJson);
        var encoded = WorkflowCheckpointCodec.Encode(snapshotJson);
        var checkpointRef = $"{session.EngineRunId ?? "run"}:{nextSequence:D4}";

        dbContext.WorkflowCheckpoints.Add(new WorkflowCheckpointEntity
        {
            Id = Guid.NewGuid(),
            WorkflowSessionId = workflowSessionId,
            Sequence = nextSequence,
            RunId = session.EngineRunId ?? string.Empty,
            CheckpointRef = checkpointRef,
            Status = session.Status == Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded ? "Completed" : session.Status.ToString(),
            CurrentNodeKey = session.CurrentNodeKey,
            SnapshotJson = snapshotJson,
            PayloadCompressed = encoded.PayloadCompressed,
            PayloadEncoding = encoded.PayloadEncoding,
            PayloadSha256 = encoded.PayloadSha256,
            PayloadSizeBytes = encoded.PayloadSizeBytes,
            PendingRequestsJson = session.ActiveReviewTaskId.HasValue
                ? JsonSerializer.Serialize(new[] { new { type = "review", reviewTaskId = session.ActiveReviewTaskId } })
                : "[]",
            AgentStateRefsJson = JsonSerializer.Serialize(new { agentSessionId = session.AgentSessionId }),
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync();
        return new CheckpointInfo(sessionId, checkpointRef);
    }

    public async ValueTask<JsonElement> RetrieveCheckpointAsync(
        string sessionId,
        CheckpointInfo key)
    {
        if (!Guid.TryParse(sessionId, out var workflowSessionId))
        {
            throw new InvalidOperationException($"Invalid workflow session id: {sessionId}");
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var checkpoint = await dbContext.WorkflowCheckpoints
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId && x.CheckpointRef == key.CheckpointId)
            .OrderByDescending(x => x.Sequence)
            .FirstAsync();

        using var document = JsonDocument.Parse(checkpoint.SnapshotJson);
        return document.RootElement.Clone();
    }

    private static string EnrichSnapshotJson(string snapshotJson, string? inputPayloadJson)
    {
        var root = JsonNode.Parse(snapshotJson)?.AsObject();
        if (root is null)
        {
            return snapshotJson;
        }

        if (TryDeserializeWorkflowCommand(inputPayloadJson) is { } command)
        {
            root["skillSelection"] = new JsonObject
            {
                ["bundleId"] = command.BundleId,
                ["bundleVersion"] = command.BundleVersion,
                ["investigationSkillId"] = command.InvestigationSkillId,
                ["investigationSkillVersion"] = command.InvestigationSkillVersion,
                ["diagnosisSkillId"] = command.DiagnosisSkillId,
                ["diagnosisSkillVersion"] = command.DiagnosisSkillVersion
            };
        }

        return root.ToJsonString(SerializerOptions);
    }

    private static DbConfigWorkflowCommand? TryDeserializeWorkflowCommand(string? inputPayloadJson)
    {
        if (string.IsNullOrWhiteSpace(inputPayloadJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<DbConfigWorkflowCommand>(inputPayloadJson, SerializerOptions);
        }
        catch
        {
            return null;
        }
    }
}

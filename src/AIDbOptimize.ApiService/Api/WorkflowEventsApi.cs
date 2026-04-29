using System.Text;
using System.Text.Json;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.ApiService.Api;

/// <summary>
/// Workflow events SSE API.
/// </summary>
internal static class WorkflowEventsApiRouteBuilderExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static IEndpointRouteBuilder MapWorkflowEventsApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/workflows/{sessionId}/events", HandleEventsAsync);
        return endpoints;
    }

    private static async Task HandleEventsAsync(
        string sessionId,
        HttpContext httpContext,
        IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(sessionId, out var workflowSessionId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .AsNoTracking()
            .Include(x => x.Connection)
            .FirstOrDefaultAsync(x => x.Id == workflowSessionId, cancellationToken);

        if (session is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        var response = httpContext.Response;
        response.StatusCode = StatusCodes.Status200OK;
        response.Headers.ContentType = "text/event-stream";
        response.Headers.CacheControl = "no-cache";
        response.Headers.Connection = "keep-alive";

        var lastEventId = ParseLastEventId(httpContext.Request.Headers["Last-Event-ID"].ToString());
        var snapshot = new
        {
            sessionId = session.Id.ToString(),
            workflowType = session.WorkflowName,
            engineType = session.EngineType,
            status = session.Status == Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded ? "Completed" : session.Status.ToString(),
            currentNode = session.CurrentNodeKey,
            connection = new
            {
                connectionId = session.ConnectionId.ToString(),
                displayName = session.Connection.DisplayName,
                engine = session.Connection.Engine.ToString(),
                databaseName = session.Connection.DatabaseName
            },
            createdAt = session.CreatedAt,
            updatedAt = session.UpdatedAt,
            completedAt = session.CompletedAt
        };

        await WriteSseEventAsync(response, "snapshot", snapshot, id: null, cancellationToken);

        var cursor = lastEventId;
        while (!cancellationToken.IsCancellationRequested)
        {
            var events = await dbContext.WorkflowEvents
                .AsNoTracking()
                .Where(x => x.WorkflowSessionId == workflowSessionId && x.SequenceNo > cursor)
                .OrderBy(x => x.SequenceNo)
                .ToListAsync(cancellationToken);

            foreach (var workflowEvent in events)
            {
                cursor = workflowEvent.SequenceNo;
                var payload = new
                {
                    sequence = workflowEvent.SequenceNo,
                    eventType = workflowEvent.EventType.ToString(),
                    eventName = workflowEvent.EventName,
                    occurredAt = workflowEvent.OccurredAt,
                    message = workflowEvent.Message,
                    payload = ParsePayload(workflowEvent.PayloadJson)
                };

                await WriteSseEventAsync(response, workflowEvent.EventName, payload, workflowEvent.SequenceNo, cancellationToken);
            }

            await WriteSseEventAsync(
                response,
                "heartbeat",
                new { timestamp = DateTimeOffset.UtcNow },
                id: null,
                cancellationToken);

            if (IsTerminal(session.Status) && events.Count == 0)
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            session = await dbContext.WorkflowSessions
                .AsNoTracking()
                .Include(x => x.Connection)
                .FirstAsync(x => x.Id == workflowSessionId, cancellationToken);
        }
    }

    private static long ParseLastEventId(string value)
    {
        return long.TryParse(value, out var parsed) ? parsed : 0;
    }

    private static object? ParsePayload(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }

    private static async Task WriteSseEventAsync(
        HttpResponse response,
        string eventName,
        object payload,
        long? id,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        if (id.HasValue)
        {
            builder.Append("id: ").Append(id.Value).AppendLine();
        }

        builder.Append("event: ").Append(eventName).AppendLine();
        builder.Append("data: ").Append(JsonSerializer.Serialize(payload, SerializerOptions)).AppendLine().AppendLine();

        await response.WriteAsync(builder.ToString(), cancellationToken);
        await response.Body.FlushAsync(cancellationToken);
    }

    private static bool IsTerminal(Domain.DbConfigOptimization.Enums.WorkflowSessionStatus status)
    {
        return status is Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded
            or Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Failed
            or Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Cancelled;
    }
}

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Http.Json;
using System.Text.Json;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Observability;
using Xunit;

namespace AIDbOptimize.ApiService.Tests;

public sealed class WorkflowTelemetryTests : IClassFixture<WorkflowApiTests.WorkflowApiFactory>
{
    private readonly WorkflowApiTests.WorkflowApiFactory _factory;

    public WorkflowTelemetryTests(WorkflowApiTests.WorkflowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task StartReviewResumeComplete_EmitsActivitiesAndCounters()
    {
        var connectionId = await _factory.SeedConnectionAsync("otel-main", DatabaseEngine.MySql);

        var activities = new ConcurrentBag<string>();
        using var activityListener = new ActivityListener
        {
            ShouldListenTo = source =>
                source.Name is AIDbOptimizeTelemetry.WorkflowName or AIDbOptimizeTelemetry.ReviewName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStopped = activity => activities.Add($"{activity.Source.Name}:{activity.OperationName}")
        };
        ActivitySource.AddActivityListener(activityListener);

        long workflowStarted = 0;
        long workflowCompleted = 0;
        long workflowResumed = 0;
        long reviewSubmitted = 0;
        double reviewResumeDuration = 0;

        using var meterListener = new MeterListener();
        meterListener.InstrumentPublished = (instrument, listener) =>
        {
            if (instrument.Meter.Name is AIDbOptimizeTelemetry.WorkflowName or AIDbOptimizeTelemetry.ReviewName)
            {
                listener.EnableMeasurementEvents(instrument);
            }
        };
        meterListener.SetMeasurementEventCallback<long>((instrument, measurement, _, _) =>
        {
            switch (instrument.Name)
            {
                case "aidbopt.workflow.started":
                    workflowStarted += measurement;
                    break;
                case "aidbopt.workflow.completed":
                    workflowCompleted += measurement;
                    break;
                case "aidbopt.workflow.resumed":
                    workflowResumed += measurement;
                    break;
                case "aidbopt.review.submitted":
                    reviewSubmitted += measurement;
                    break;
            }
        });
        meterListener.SetMeasurementEventCallback<double>((instrument, measurement, _, _) =>
        {
            if (instrument.Name == "aidbopt.review.resume.duration")
            {
                reviewResumeDuration = Math.Max(reviewResumeDuration, measurement);
            }
        });
        meterListener.Start();

        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = true,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "otel"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        var taskId = await WaitForReviewTaskAsync(client, workflowId!, 60);

        var submitResponse = await client.PostAsJsonAsync($"/api/reviews/{taskId}/submit", new
        {
            action = "approve",
            reviewer = "otel-tester",
            comment = "approved"
        });

        submitResponse.EnsureSuccessStatusCode();
        meterListener.RecordObservableInstruments();

        Assert.Contains($"{AIDbOptimizeTelemetry.WorkflowName}:workflow.start", activities);
        Assert.Contains($"{AIDbOptimizeTelemetry.ReviewName}:review.submit", activities);
        Assert.Contains($"{AIDbOptimizeTelemetry.WorkflowName}:workflow.resume", activities);
        Assert.True(workflowStarted >= 1);
        Assert.True(workflowCompleted >= 1);
        Assert.True(workflowResumed >= 1);
        Assert.True(reviewSubmitted >= 1);
        Assert.True(reviewResumeDuration > 0);
    }

    private static async Task<string> WaitForReviewTaskAsync(HttpClient client, string workflowId, int timeoutSeconds)
    {
        var deadline = DateTimeOffset.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTimeOffset.UtcNow < deadline)
        {
            var response = await client.GetAsync("/api/reviews");
            response.EnsureSuccessStatusCode();
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var match = payload.RootElement
                .EnumerateArray()
                .FirstOrDefault(item => string.Equals(item.GetProperty("sessionId").GetString(), workflowId, StringComparison.OrdinalIgnoreCase));
            if (match.ValueKind == JsonValueKind.Object &&
                match.TryGetProperty("taskId", out var taskIdElement) &&
                !string.IsNullOrWhiteSpace(taskIdElement.GetString()))
            {
                return taskIdElement.GetString()!;
            }

            await Task.Delay(500);
        }

        throw new TimeoutException($"Workflow {workflowId} did not create a review task in time.");
    }
}

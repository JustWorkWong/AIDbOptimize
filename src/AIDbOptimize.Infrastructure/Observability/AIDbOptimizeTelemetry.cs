using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace AIDbOptimize.Infrastructure.Observability;

/// <summary>
/// AIDbOptimize 统一观测入口。
/// </summary>
public static class AIDbOptimizeTelemetry
{
    public const string WorkflowName = "AIDbOptimize.Workflow";
    public const string AgentName = "AIDbOptimize.Agent";
    public const string ReviewName = "AIDbOptimize.Review";
    public const string McpName = "AIDbOptimize.Mcp";

    public static readonly ActivitySource WorkflowActivitySource = new(WorkflowName);
    public static readonly ActivitySource AgentActivitySource = new(AgentName);
    public static readonly ActivitySource ReviewActivitySource = new(ReviewName);
    public static readonly ActivitySource McpActivitySource = new(McpName);

    public static readonly Meter WorkflowMeter = new(WorkflowName);
    public static readonly Meter AgentMeter = new(AgentName);
    public static readonly Meter ReviewMeter = new(ReviewName);
    public static readonly Meter McpMeter = new(McpName);

    public static readonly Counter<long> WorkflowStarted = WorkflowMeter.CreateCounter<long>(
        "aidbopt.workflow.started",
        description: "Number of workflow starts");

    public static readonly Counter<long> WorkflowCompleted = WorkflowMeter.CreateCounter<long>(
        "aidbopt.workflow.completed",
        description: "Number of workflow completions");

    public static readonly Counter<long> WorkflowCancelled = WorkflowMeter.CreateCounter<long>(
        "aidbopt.workflow.cancelled",
        description: "Number of workflow cancellations");

    public static readonly Counter<long> WorkflowResumed = WorkflowMeter.CreateCounter<long>(
        "aidbopt.workflow.resumed",
        description: "Number of workflow resumes");

    public static readonly Counter<long> WorkflowFailed = WorkflowMeter.CreateCounter<long>(
        "aidbopt.workflow.failed",
        description: "Number of workflow failures");

    public static readonly Counter<long> CheckpointSaved = WorkflowMeter.CreateCounter<long>(
        "aidbopt.workflow.checkpoint.saved",
        description: "Number of persisted workflow checkpoints");

    public static readonly Histogram<long> CheckpointSnapshotBytes = WorkflowMeter.CreateHistogram<long>(
        "aidbopt.workflow.checkpoint.snapshot_bytes",
        unit: "By",
        description: "Serialized workflow checkpoint size");

    public static readonly Histogram<double> WorkflowDurationMs = WorkflowMeter.CreateHistogram<double>(
        "aidbopt.workflow.duration",
        unit: "ms",
        description: "Workflow operation duration");

    public static readonly Counter<long> ReviewSubmitted = ReviewMeter.CreateCounter<long>(
        "aidbopt.review.submitted",
        description: "Number of review submissions");

    public static readonly Counter<long> ReviewAdjusted = ReviewMeter.CreateCounter<long>(
        "aidbopt.review.adjusted",
        description: "Number of adjusted reviews");

    public static readonly Histogram<double> ReviewResumeDurationMs = ReviewMeter.CreateHistogram<double>(
        "aidbopt.review.resume.duration",
        unit: "ms",
        description: "Duration from review submit to workflow resume completion");

    public static readonly Counter<long> AgentSessionCreated = AgentMeter.CreateCounter<long>(
        "aidbopt.agent.session.created",
        description: "Number of persisted agent sessions");

    public static readonly Counter<long> AgentSummaryGenerated = AgentMeter.CreateCounter<long>(
        "aidbopt.agent.summary.generated",
        description: "Number of generated rolling summaries");

    public static readonly Counter<long> AgentMessagesPersisted = AgentMeter.CreateCounter<long>(
        "aidbopt.agent.messages.persisted",
        description: "Number of persisted agent messages");

    public static readonly Counter<long> McpToolExecutionStarted = McpMeter.CreateCounter<long>(
        "aidbopt.mcp.tool.started",
        description: "Number of MCP tool executions");

    public static readonly Counter<long> McpToolExecutionFailed = McpMeter.CreateCounter<long>(
        "aidbopt.mcp.tool.failed",
        description: "Number of failed MCP tool executions");

    public static readonly Histogram<double> McpToolExecutionDurationMs = McpMeter.CreateHistogram<double>(
        "aidbopt.mcp.tool.duration",
        unit: "ms",
        description: "Duration of MCP tool execution");
}

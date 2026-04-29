using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.ControlPlane
{
    /// <inheritdoc />
    public partial class AlignWorkflowTargetState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActiveReviewTaskId",
                table: "workflow_sessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentSessionId",
                table: "workflow_sessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineCheckpointRef",
                table: "workflow_sessions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineRunId",
                table: "workflow_sessions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineStateJson",
                table: "workflow_sessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineType",
                table: "workflow_sessions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "workflow_sessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedCost",
                table: "workflow_sessions",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ResultType",
                table: "workflow_sessions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateJson",
                table: "workflow_sessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TotalTokens",
                table: "workflow_sessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AdjustmentsJson",
                table: "workflow_review_tasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CheckpointId",
                table: "workflow_review_tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineCheckpointRef",
                table: "workflow_review_tasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineRunId",
                table: "workflow_review_tasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestId",
                table: "workflow_review_tasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentSessionId",
                table: "workflow_node_executions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenUsageJson",
                table: "workflow_node_executions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SequenceNo",
                table: "workflow_events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SpanId",
                table: "workflow_events",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraceId",
                table: "workflow_events",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentStateRefsJson",
                table: "workflow_checkpoints",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CheckpointRef",
                table: "workflow_checkpoints",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayloadCompressed",
                table: "workflow_checkpoints",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayloadEncoding",
                table: "workflow_checkpoints",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayloadSha256",
                table: "workflow_checkpoints",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PayloadSizeBytes",
                table: "workflow_checkpoints",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PendingRequestsJson",
                table: "workflow_checkpoints",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RunId",
                table: "workflow_checkpoints",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExecutionScope",
                table: "mcp_tool_executions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TraceId",
                table: "mcp_tool_executions",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowNodeName",
                table: "mcp_tool_executions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_workflow_sessions_ActiveReviewTaskId",
                table: "workflow_sessions",
                column: "ActiveReviewTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_sessions_AgentSessionId",
                table: "workflow_sessions",
                column: "AgentSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_review_tasks_CheckpointId",
                table: "workflow_review_tasks",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_node_executions_AgentSessionId",
                table: "workflow_node_executions",
                column: "AgentSessionId");

            migrationBuilder.CreateIndex(
                name: "idx_workflow_events_session_sequence",
                table: "workflow_events",
                columns: new[] { "WorkflowSessionId", "SequenceNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_workflow_checkpoints_session_ref",
                table: "workflow_checkpoints",
                columns: new[] { "WorkflowSessionId", "CheckpointRef" });

            migrationBuilder.CreateIndex(
                name: "idx_mcp_tool_executions_session_node",
                table: "mcp_tool_executions",
                columns: new[] { "WorkflowSessionId", "WorkflowNodeName" });

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_node_executions_agent_sessions_AgentSessionId",
                table: "workflow_node_executions",
                column: "AgentSessionId",
                principalTable: "agent_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_review_tasks_workflow_checkpoints_CheckpointId",
                table: "workflow_review_tasks",
                column: "CheckpointId",
                principalTable: "workflow_checkpoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_sessions_agent_sessions_AgentSessionId",
                table: "workflow_sessions",
                column: "AgentSessionId",
                principalTable: "agent_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_sessions_workflow_review_tasks_ActiveReviewTaskId",
                table: "workflow_sessions",
                column: "ActiveReviewTaskId",
                principalTable: "workflow_review_tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workflow_node_executions_agent_sessions_AgentSessionId",
                table: "workflow_node_executions");

            migrationBuilder.DropForeignKey(
                name: "FK_workflow_review_tasks_workflow_checkpoints_CheckpointId",
                table: "workflow_review_tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_workflow_sessions_agent_sessions_AgentSessionId",
                table: "workflow_sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_workflow_sessions_workflow_review_tasks_ActiveReviewTaskId",
                table: "workflow_sessions");

            migrationBuilder.DropIndex(
                name: "IX_workflow_sessions_ActiveReviewTaskId",
                table: "workflow_sessions");

            migrationBuilder.DropIndex(
                name: "IX_workflow_sessions_AgentSessionId",
                table: "workflow_sessions");

            migrationBuilder.DropIndex(
                name: "IX_workflow_review_tasks_CheckpointId",
                table: "workflow_review_tasks");

            migrationBuilder.DropIndex(
                name: "IX_workflow_node_executions_AgentSessionId",
                table: "workflow_node_executions");

            migrationBuilder.DropIndex(
                name: "idx_workflow_events_session_sequence",
                table: "workflow_events");

            migrationBuilder.DropIndex(
                name: "idx_workflow_checkpoints_session_ref",
                table: "workflow_checkpoints");

            migrationBuilder.DropIndex(
                name: "idx_mcp_tool_executions_session_node",
                table: "mcp_tool_executions");

            migrationBuilder.DropColumn(
                name: "ActiveReviewTaskId",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "AgentSessionId",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "EngineCheckpointRef",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "EngineRunId",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "EngineStateJson",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "EngineType",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "EstimatedCost",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "ResultType",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "StateJson",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "TotalTokens",
                table: "workflow_sessions");

            migrationBuilder.DropColumn(
                name: "AdjustmentsJson",
                table: "workflow_review_tasks");

            migrationBuilder.DropColumn(
                name: "CheckpointId",
                table: "workflow_review_tasks");

            migrationBuilder.DropColumn(
                name: "EngineCheckpointRef",
                table: "workflow_review_tasks");

            migrationBuilder.DropColumn(
                name: "EngineRunId",
                table: "workflow_review_tasks");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "workflow_review_tasks");

            migrationBuilder.DropColumn(
                name: "AgentSessionId",
                table: "workflow_node_executions");

            migrationBuilder.DropColumn(
                name: "TokenUsageJson",
                table: "workflow_node_executions");

            migrationBuilder.DropColumn(
                name: "SequenceNo",
                table: "workflow_events");

            migrationBuilder.DropColumn(
                name: "SpanId",
                table: "workflow_events");

            migrationBuilder.DropColumn(
                name: "TraceId",
                table: "workflow_events");

            migrationBuilder.DropColumn(
                name: "AgentStateRefsJson",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "CheckpointRef",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "PayloadCompressed",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "PayloadEncoding",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "PayloadSha256",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "PayloadSizeBytes",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "PendingRequestsJson",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "RunId",
                table: "workflow_checkpoints");

            migrationBuilder.DropColumn(
                name: "ExecutionScope",
                table: "mcp_tool_executions");

            migrationBuilder.DropColumn(
                name: "TraceId",
                table: "mcp_tool_executions");

            migrationBuilder.DropColumn(
                name: "WorkflowNodeName",
                table: "mcp_tool_executions");
        }
    }
}

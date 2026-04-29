using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.ControlPlane
{
    /// <inheritdoc />
    public partial class AddWorkflowControlPlane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowSessionId",
                table: "mcp_tool_executions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "workflow_sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InputPayloadJson = table.Column<string>(type: "text", nullable: false),
                    ResultPayloadJson = table.Column<string>(type: "text", nullable: false),
                    CurrentNodeKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_sessions_mcp_connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "mcp_connections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workflow_node_executions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NodeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Attempt = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    InputPayloadJson = table.Column<string>(type: "text", nullable: false),
                    OutputPayloadJson = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_node_executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_node_executions_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_review_tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeExecutionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DecisionBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DecisionNote = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_review_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_review_tasks_workflow_node_executions_NodeExecutio~",
                        column: x => x.NodeExecutionId,
                        principalTable: "workflow_node_executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_workflow_review_tasks_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeExecutionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    McpToolExecutionId = table.Column<Guid>(type: "uuid", nullable: true),
                    EventType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    EventName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_events_mcp_tool_executions_McpToolExecutionId",
                        column: x => x.McpToolExecutionId,
                        principalTable: "mcp_tool_executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_workflow_events_workflow_node_executions_NodeExecutionId",
                        column: x => x.NodeExecutionId,
                        principalTable: "workflow_node_executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_workflow_events_workflow_review_tasks_ReviewTaskId",
                        column: x => x.ReviewTaskId,
                        principalTable: "workflow_review_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_workflow_events_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mcp_tool_executions_WorkflowSessionId",
                table: "mcp_tool_executions",
                column: "WorkflowSessionId");

            migrationBuilder.CreateIndex(
                name: "idx_workflow_events_session_occurred_at",
                table: "workflow_events",
                columns: new[] { "WorkflowSessionId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_workflow_events_McpToolExecutionId",
                table: "workflow_events",
                column: "McpToolExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_events_NodeExecutionId",
                table: "workflow_events",
                column: "NodeExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_events_ReviewTaskId",
                table: "workflow_events",
                column: "ReviewTaskId");

            migrationBuilder.CreateIndex(
                name: "idx_workflow_node_executions_session_node_attempt",
                table: "workflow_node_executions",
                columns: new[] { "WorkflowSessionId", "NodeKey", "Attempt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_workflow_review_tasks_session_status",
                table: "workflow_review_tasks",
                columns: new[] { "WorkflowSessionId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_workflow_review_tasks_NodeExecutionId",
                table: "workflow_review_tasks",
                column: "NodeExecutionId");

            migrationBuilder.CreateIndex(
                name: "idx_workflow_sessions_connection_status",
                table: "workflow_sessions",
                columns: new[] { "ConnectionId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_workflow_sessions_ConnectionId",
                table: "workflow_sessions",
                column: "ConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_mcp_tool_executions_workflow_sessions_WorkflowSessionId",
                table: "mcp_tool_executions",
                column: "WorkflowSessionId",
                principalTable: "workflow_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mcp_tool_executions_workflow_sessions_WorkflowSessionId",
                table: "mcp_tool_executions");

            migrationBuilder.DropTable(
                name: "workflow_events");

            migrationBuilder.DropTable(
                name: "workflow_review_tasks");

            migrationBuilder.DropTable(
                name: "workflow_node_executions");

            migrationBuilder.DropTable(
                name: "workflow_sessions");

            migrationBuilder.DropIndex(
                name: "IX_mcp_tool_executions_WorkflowSessionId",
                table: "mcp_tool_executions");

            migrationBuilder.DropColumn(
                name: "WorkflowSessionId",
                table: "mcp_tool_executions");
        }
    }
}

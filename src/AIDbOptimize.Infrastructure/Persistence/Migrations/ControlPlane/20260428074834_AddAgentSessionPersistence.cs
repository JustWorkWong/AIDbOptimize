using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.ControlPlane
{
    /// <inheritdoc />
    public partial class AddAgentSessionPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agent_messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SequenceNo = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MessageKind = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    RawPayloadJson = table.Column<string>(type: "text", nullable: false),
                    TraceId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agent_messages_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "agent_sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentRole = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SerializedSessionJson = table.Column<string>(type: "text", nullable: false),
                    SessionStateJson = table.Column<string>(type: "text", nullable: false),
                    ActiveSummaryId = table.Column<Guid>(type: "uuid", nullable: true),
                    PromptVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModelId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MessageGroupCount = table.Column<int>(type: "integer", nullable: false),
                    LastCompactedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agent_sessions_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "agent_summaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SummaryType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SummaryJson = table.Column<string>(type: "text", nullable: false),
                    SourceStartSequence = table.Column<long>(type: "bigint", nullable: false),
                    SourceEndSequence = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_summaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agent_summaries_agent_sessions_AgentSessionId",
                        column: x => x.AgentSessionId,
                        principalTable: "agent_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_agent_messages_session_sequence",
                table: "agent_messages",
                columns: new[] { "AgentSessionId", "SequenceNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agent_messages_WorkflowSessionId",
                table: "agent_messages",
                column: "WorkflowSessionId");

            migrationBuilder.CreateIndex(
                name: "idx_agent_sessions_workflow_session_id",
                table: "agent_sessions",
                column: "WorkflowSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_agent_sessions_ActiveSummaryId",
                table: "agent_sessions",
                column: "ActiveSummaryId");

            migrationBuilder.CreateIndex(
                name: "idx_agent_summaries_agent_session_id",
                table: "agent_summaries",
                column: "AgentSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_agent_messages_agent_sessions_AgentSessionId",
                table: "agent_messages",
                column: "AgentSessionId",
                principalTable: "agent_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_agent_sessions_agent_summaries_ActiveSummaryId",
                table: "agent_sessions",
                column: "ActiveSummaryId",
                principalTable: "agent_summaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_agent_summaries_agent_sessions_AgentSessionId",
                table: "agent_summaries");

            migrationBuilder.DropTable(
                name: "agent_messages");

            migrationBuilder.DropTable(
                name: "agent_sessions");

            migrationBuilder.DropTable(
                name: "agent_summaries");
        }
    }
}

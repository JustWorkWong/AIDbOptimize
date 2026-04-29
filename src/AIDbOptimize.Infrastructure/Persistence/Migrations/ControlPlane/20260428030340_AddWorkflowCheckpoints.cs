using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.ControlPlane
{
    /// <inheritdoc />
    public partial class AddWorkflowCheckpoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "workflow_checkpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CurrentNodeKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SnapshotJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_checkpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_checkpoints_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_workflow_checkpoints_session_sequence",
                table: "workflow_checkpoints",
                columns: new[] { "WorkflowSessionId", "Sequence" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workflow_checkpoints");
        }
    }
}

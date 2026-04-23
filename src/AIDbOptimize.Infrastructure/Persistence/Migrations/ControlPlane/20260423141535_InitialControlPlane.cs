using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.ControlPlane
{
    /// <inheritdoc />
    public partial class InitialControlPlane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_initialization_runs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Engine = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DatabaseName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SeedVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetOrderCount = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_initialization_runs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcp_connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Engine = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ServerCommand = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ServerArgumentsJson = table.Column<string>(type: "text", nullable: false),
                    EnvironmentJson = table.Column<string>(type: "text", nullable: false),
                    DatabaseConnectionString = table.Column<string>(type: "text", nullable: false),
                    DatabaseName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastDiscoveredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcp_connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcp_tools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    InputSchemaJson = table.Column<string>(type: "text", nullable: false),
                    ApprovalMode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsWriteTool = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcp_tools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mcp_tools_mcp_connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "mcp_connections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mcp_tool_executions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RequestPayloadJson = table.Column<string>(type: "text", nullable: false),
                    ResponsePayloadJson = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcp_tool_executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mcp_tool_executions_mcp_connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "mcp_connections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mcp_tool_executions_mcp_tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "mcp_tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_initialization_runs_Engine_DatabaseName_SeedVersion",
                table: "data_initialization_runs",
                columns: new[] { "Engine", "DatabaseName", "SeedVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_mcp_connections_engine",
                table: "mcp_connections",
                column: "Engine");

            migrationBuilder.CreateIndex(
                name: "idx_mcp_connections_is_default",
                table: "mcp_connections",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_mcp_tool_executions_ConnectionId",
                table: "mcp_tool_executions",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_mcp_tool_executions_ToolId",
                table: "mcp_tool_executions",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_mcp_tools_ConnectionId_ToolName",
                table: "mcp_tools",
                columns: new[] { "ConnectionId", "ToolName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_initialization_runs");

            migrationBuilder.DropTable(
                name: "mcp_tool_executions");

            migrationBuilder.DropTable(
                name: "mcp_tools");

            migrationBuilder.DropTable(
                name: "mcp_connections");
        }
    }
}

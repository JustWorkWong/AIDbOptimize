using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.ControlPlane
{
    /// <inheritdoc />
    public partial class AddRagControlPlaneSkeleton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rag_case_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Engine = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProblemType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Outcome = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ReviewStatus = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    RecommendationType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rag_case_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rag_case_records_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rag_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Engine = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Vendor = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Topic = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SourcePath = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SourceUrl = table.Column<string>(type: "text", nullable: false),
                    SourceTitle = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ContentHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CapturedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rag_documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rag_retrieval_snapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeExecutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SnapshotTypeJson = table.Column<string>(type: "text", nullable: false),
                    RetrievedItemsJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rag_retrieval_snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rag_retrieval_snapshots_workflow_node_executions_NodeExecut~",
                        column: x => x.NodeExecutionId,
                        principalTable: "workflow_node_executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rag_retrieval_snapshots_workflow_sessions_WorkflowSessionId",
                        column: x => x.WorkflowSessionId,
                        principalTable: "workflow_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rag_case_evidence_links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CaseRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    EvidenceReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RecommendationKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rag_case_evidence_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rag_case_evidence_links_rag_case_records_CaseRecordId",
                        column: x => x.CaseRecordId,
                        principalTable: "rag_case_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rag_document_chunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChunkKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SectionPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ProductVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AppliesTo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ParameterNamesJson = table.Column<string>(type: "text", nullable: false),
                    KeywordsJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rag_document_chunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rag_document_chunks_rag_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "rag_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_rag_case_evidence_links_case_record",
                table: "rag_case_evidence_links",
                column: "CaseRecordId");

            migrationBuilder.CreateIndex(
                name: "idx_rag_case_records_workflow_session",
                table: "rag_case_records",
                column: "WorkflowSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_rag_document_chunks_document_chunk_key",
                table: "rag_document_chunks",
                columns: new[] { "DocumentId", "ChunkKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_rag_documents_engine_vendor_topic",
                table: "rag_documents",
                columns: new[] { "Engine", "Vendor", "Topic" });

            migrationBuilder.CreateIndex(
                name: "idx_rag_documents_source_path",
                table: "rag_documents",
                column: "SourcePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_rag_retrieval_snapshots_node",
                table: "rag_retrieval_snapshots",
                column: "NodeExecutionId");

            migrationBuilder.CreateIndex(
                name: "idx_rag_retrieval_snapshots_session",
                table: "rag_retrieval_snapshots",
                column: "WorkflowSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rag_case_evidence_links");

            migrationBuilder.DropTable(
                name: "rag_document_chunks");

            migrationBuilder.DropTable(
                name: "rag_retrieval_snapshots");

            migrationBuilder.DropTable(
                name: "rag_case_records");

            migrationBuilder.DropTable(
                name: "rag_documents");
        }
    }
}

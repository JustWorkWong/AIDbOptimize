using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Seed.Enums;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// Control-plane database context.
/// </summary>
public sealed class ControlPlaneDbContext(DbContextOptions<ControlPlaneDbContext> options) : DbContext(options)
{
    public DbSet<McpConnectionEntity> McpConnections => Set<McpConnectionEntity>();
    public DbSet<McpToolEntity> McpTools => Set<McpToolEntity>();
    public DbSet<McpToolExecutionEntity> McpToolExecutions => Set<McpToolExecutionEntity>();
    public DbSet<WorkflowSessionEntity> WorkflowSessions => Set<WorkflowSessionEntity>();
    public DbSet<WorkflowCheckpointEntity> WorkflowCheckpoints => Set<WorkflowCheckpointEntity>();
    public DbSet<WorkflowReviewTaskEntity> WorkflowReviewTasks => Set<WorkflowReviewTaskEntity>();
    public DbSet<WorkflowNodeExecutionEntity> WorkflowNodeExecutions => Set<WorkflowNodeExecutionEntity>();
    public DbSet<WorkflowEventEntity> WorkflowEvents => Set<WorkflowEventEntity>();
    public DbSet<AgentSessionEntity> AgentSessions => Set<AgentSessionEntity>();
    public DbSet<AgentSummaryEntity> AgentSummaries => Set<AgentSummaryEntity>();
    public DbSet<AgentMessageEntity> AgentMessages => Set<AgentMessageEntity>();
    public DbSet<DataInitializationRunEntity> DataInitializationRuns => Set<DataInitializationRunEntity>();
    public DbSet<RagDocumentEntity> RagDocuments => Set<RagDocumentEntity>();
    public DbSet<RagDocumentChunkEntity> RagDocumentChunks => Set<RagDocumentChunkEntity>();
    public DbSet<RagCaseRecordEntity> RagCaseRecords => Set<RagCaseRecordEntity>();
    public DbSet<RagCaseEvidenceLinkEntity> RagCaseEvidenceLinks => Set<RagCaseEvidenceLinkEntity>();
    public DbSet<RagRetrievalSnapshotEntity> RagRetrievalSnapshots => Set<RagRetrievalSnapshotEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        ConfigureMcpConnections(modelBuilder);
        ConfigureMcpTools(modelBuilder);
        ConfigureMcpToolExecutions(modelBuilder);
        ConfigureWorkflowSessions(modelBuilder);
        ConfigureWorkflowCheckpoints(modelBuilder);
        ConfigureWorkflowNodeExecutions(modelBuilder);
        ConfigureWorkflowReviewTasks(modelBuilder);
        ConfigureWorkflowEvents(modelBuilder);
        ConfigureAgentSessions(modelBuilder);
        ConfigureAgentSummaries(modelBuilder);
        ConfigureAgentMessages(modelBuilder);
        ConfigureDataInitializationRuns(modelBuilder);
        ConfigureRagDocuments(modelBuilder);
        ConfigureRagDocumentChunks(modelBuilder);
        ConfigureRagCaseRecords(modelBuilder);
        ConfigureRagCaseEvidenceLinks(modelBuilder);
        ConfigureRagRetrievalSnapshots(modelBuilder);
    }

    private static void ConfigureMcpConnections(ModelBuilder modelBuilder)
    {
        var connection = modelBuilder.Entity<McpConnectionEntity>();
        connection.ToTable("mcp_connections");
        connection.HasKey(x => x.Id);
        connection.Property(x => x.Name).HasMaxLength(100).IsRequired();
        connection.Property(x => x.Engine).HasConversion<string>().HasMaxLength(20);
        connection.Property(x => x.DisplayName).HasMaxLength(100).IsRequired();
        connection.Property(x => x.ServerCommand).HasMaxLength(500).IsRequired();
        connection.Property(x => x.DatabaseName).HasMaxLength(100).IsRequired();
        connection.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        connection.HasIndex(x => x.Engine).HasDatabaseName("idx_mcp_connections_engine");
        connection.HasIndex(x => x.IsDefault).HasDatabaseName("idx_mcp_connections_is_default");
    }

    private static void ConfigureMcpTools(ModelBuilder modelBuilder)
    {
        var tool = modelBuilder.Entity<McpToolEntity>();
        tool.ToTable("mcp_tools");
        tool.HasKey(x => x.Id);
        tool.Property(x => x.ToolName).HasMaxLength(200).IsRequired();
        tool.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        tool.Property(x => x.ApprovalMode).HasConversion<string>().HasMaxLength(30);
        tool.HasIndex(x => new { x.ConnectionId, x.ToolName }).IsUnique();
        tool.HasOne(x => x.Connection)
            .WithMany(x => x.Tools)
            .HasForeignKey(x => x.ConnectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureMcpToolExecutions(ModelBuilder modelBuilder)
    {
        var execution = modelBuilder.Entity<McpToolExecutionEntity>();
        execution.ToTable("mcp_tool_executions");
        execution.HasKey(x => x.Id);
        execution.Property(x => x.RequestedBy).HasMaxLength(100).IsRequired();
        execution.Property(x => x.WorkflowNodeName).HasMaxLength(100);
        execution.Property(x => x.ExecutionScope).HasMaxLength(20).IsRequired();
        execution.Property(x => x.TraceId).HasMaxLength(64);
        execution.Property(x => x.Status).HasMaxLength(20).IsRequired();
        execution.HasIndex(x => x.WorkflowSessionId);
        execution.HasIndex(x => new { x.WorkflowSessionId, x.WorkflowNodeName })
            .HasDatabaseName("idx_mcp_tool_executions_session_node");
        execution.HasOne(x => x.Connection)
            .WithMany()
            .HasForeignKey(x => x.ConnectionId)
            .OnDelete(DeleteBehavior.Restrict);
        execution.HasOne(x => x.Tool)
            .WithMany(x => x.Executions)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);
        execution.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.ToolExecutions)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureWorkflowSessions(ModelBuilder modelBuilder)
    {
        var session = modelBuilder.Entity<WorkflowSessionEntity>();
        session.ToTable("workflow_sessions");
        session.HasKey(x => x.Id);
        session.Property(x => x.WorkflowName).HasMaxLength(100).IsRequired();
        session.Property(x => x.EngineType).HasMaxLength(20).IsRequired();
        session.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        session.Property(x => x.RequestedBy).HasMaxLength(100).IsRequired();
        session.Property(x => x.ResultType).HasMaxLength(100).IsRequired();
        session.Property(x => x.CurrentNodeKey).HasMaxLength(100);
        session.Property(x => x.EngineRunId).HasMaxLength(100);
        session.Property(x => x.EngineCheckpointRef).HasMaxLength(100);
        session.Property(x => x.ErrorMessage).HasColumnType("text");
        session.Property(x => x.EstimatedCost).HasColumnType("numeric(18,4)");
        session.HasIndex(x => x.ConnectionId);
        session.HasIndex(x => new { x.ConnectionId, x.Status })
            .HasDatabaseName("idx_workflow_sessions_connection_status");
        session.HasOne(x => x.Connection)
            .WithMany()
            .HasForeignKey(x => x.ConnectionId)
            .OnDelete(DeleteBehavior.Restrict);
        session.HasOne(x => x.ActiveReviewTask)
            .WithMany()
            .HasForeignKey(x => x.ActiveReviewTaskId)
            .OnDelete(DeleteBehavior.SetNull);
        session.HasOne(x => x.PrimaryAgentSession)
            .WithMany()
            .HasForeignKey(x => x.AgentSessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureWorkflowCheckpoints(ModelBuilder modelBuilder)
    {
        var checkpoint = modelBuilder.Entity<WorkflowCheckpointEntity>();
        checkpoint.ToTable("workflow_checkpoints");
        checkpoint.HasKey(x => x.Id);
        checkpoint.Property(x => x.RunId).HasMaxLength(100).IsRequired();
        checkpoint.Property(x => x.CheckpointRef).HasMaxLength(100).IsRequired();
        checkpoint.Property(x => x.Status).HasMaxLength(30).IsRequired();
        checkpoint.Property(x => x.CurrentNodeKey).HasMaxLength(100);
        checkpoint.Property(x => x.PayloadEncoding).HasMaxLength(30).IsRequired();
        checkpoint.Property(x => x.PayloadSha256).HasMaxLength(128).IsRequired();
        checkpoint.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.Checkpoints)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        checkpoint.HasIndex(x => new { x.WorkflowSessionId, x.Sequence })
            .IsUnique()
            .HasDatabaseName("idx_workflow_checkpoints_session_sequence");
        checkpoint.HasIndex(x => new { x.WorkflowSessionId, x.CheckpointRef })
            .HasDatabaseName("idx_workflow_checkpoints_session_ref");
    }

    private static void ConfigureWorkflowNodeExecutions(ModelBuilder modelBuilder)
    {
        var execution = modelBuilder.Entity<WorkflowNodeExecutionEntity>();
        execution.ToTable("workflow_node_executions");
        execution.HasKey(x => x.Id);
        execution.Property(x => x.NodeKey).HasMaxLength(100).IsRequired();
        execution.Property(x => x.NodeType).HasMaxLength(100).IsRequired();
        execution.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        execution.HasIndex(x => new { x.WorkflowSessionId, x.NodeKey, x.Attempt })
            .IsUnique()
            .HasDatabaseName("idx_workflow_node_executions_session_node_attempt");
        execution.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.NodeExecutions)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        execution.HasOne(x => x.AgentSession)
            .WithMany(x => x.NodeExecutions)
            .HasForeignKey(x => x.AgentSessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureWorkflowReviewTasks(ModelBuilder modelBuilder)
    {
        var task = modelBuilder.Entity<WorkflowReviewTaskEntity>();
        task.ToTable("workflow_review_tasks");
        task.HasKey(x => x.Id);
        task.Property(x => x.Title).HasMaxLength(200).IsRequired();
        task.Property(x => x.RequestId).HasMaxLength(100);
        task.Property(x => x.EngineRunId).HasMaxLength(100);
        task.Property(x => x.EngineCheckpointRef).HasMaxLength(100);
        task.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        task.Property(x => x.RequestedBy).HasMaxLength(100).IsRequired();
        task.Property(x => x.DecisionBy).HasMaxLength(100);
        task.HasIndex(x => new { x.WorkflowSessionId, x.Status })
            .HasDatabaseName("idx_workflow_review_tasks_session_status");
        task.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.ReviewTasks)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        task.HasOne(x => x.NodeExecution)
            .WithMany(x => x.ReviewTasks)
            .HasForeignKey(x => x.NodeExecutionId)
            .OnDelete(DeleteBehavior.SetNull);
        task.HasOne(x => x.Checkpoint)
            .WithMany()
            .HasForeignKey(x => x.CheckpointId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureWorkflowEvents(ModelBuilder modelBuilder)
    {
        var workflowEvent = modelBuilder.Entity<WorkflowEventEntity>();
        workflowEvent.ToTable("workflow_events");
        workflowEvent.HasKey(x => x.Id);
        workflowEvent.Property(x => x.EventType).HasConversion<string>().HasMaxLength(40);
        workflowEvent.Property(x => x.EventName).HasMaxLength(100).IsRequired();
        workflowEvent.Property(x => x.Message).HasMaxLength(500);
        workflowEvent.Property(x => x.TraceId).HasMaxLength(64);
        workflowEvent.Property(x => x.SpanId).HasMaxLength(32);
        workflowEvent.HasIndex(x => new { x.WorkflowSessionId, x.SequenceNo })
            .IsUnique()
            .HasDatabaseName("idx_workflow_events_session_sequence");
        workflowEvent.HasIndex(x => new { x.WorkflowSessionId, x.OccurredAt })
            .HasDatabaseName("idx_workflow_events_session_occurred_at");
        workflowEvent.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        workflowEvent.HasOne(x => x.NodeExecution)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.NodeExecutionId)
            .OnDelete(DeleteBehavior.SetNull);
        workflowEvent.HasOne(x => x.ReviewTask)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.ReviewTaskId)
            .OnDelete(DeleteBehavior.SetNull);
        workflowEvent.HasOne(x => x.McpToolExecution)
            .WithMany(x => x.WorkflowEvents)
            .HasForeignKey(x => x.McpToolExecutionId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureAgentSessions(ModelBuilder modelBuilder)
    {
        var agentSession = modelBuilder.Entity<AgentSessionEntity>();
        agentSession.ToTable("agent_sessions");
        agentSession.HasKey(x => x.Id);
        agentSession.Property(x => x.AgentRole).HasMaxLength(100).IsRequired();
        agentSession.Property(x => x.PromptVersion).HasMaxLength(100).IsRequired();
        agentSession.Property(x => x.ModelId).HasMaxLength(100).IsRequired();
        agentSession.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.AgentSessions)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        agentSession.HasOne(x => x.ActiveSummary)
            .WithMany()
            .HasForeignKey(x => x.ActiveSummaryId)
            .OnDelete(DeleteBehavior.SetNull);
        agentSession.HasIndex(x => x.WorkflowSessionId)
            .HasDatabaseName("idx_agent_sessions_workflow_session_id");
    }

    private static void ConfigureAgentSummaries(ModelBuilder modelBuilder)
    {
        var summary = modelBuilder.Entity<AgentSummaryEntity>();
        summary.ToTable("agent_summaries");
        summary.HasKey(x => x.Id);
        summary.Property(x => x.SummaryType).HasMaxLength(50).IsRequired();
        summary.HasOne(x => x.AgentSession)
            .WithMany(x => x.Summaries)
            .HasForeignKey(x => x.AgentSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        summary.HasIndex(x => x.AgentSessionId)
            .HasDatabaseName("idx_agent_summaries_agent_session_id");
    }

    private static void ConfigureAgentMessages(ModelBuilder modelBuilder)
    {
        var message = modelBuilder.Entity<AgentMessageEntity>();
        message.ToTable("agent_messages");
        message.HasKey(x => x.Id);
        message.Property(x => x.Role).HasMaxLength(20).IsRequired();
        message.Property(x => x.MessageKind).HasMaxLength(50).IsRequired();
        message.Property(x => x.TraceId).HasMaxLength(64);
        message.HasOne(x => x.AgentSession)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.AgentSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        message.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.AgentMessages)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        message.HasIndex(x => new { x.AgentSessionId, x.SequenceNo })
            .IsUnique()
            .HasDatabaseName("idx_agent_messages_session_sequence");
    }

    private static void ConfigureDataInitializationRuns(ModelBuilder modelBuilder)
    {
        var init = modelBuilder.Entity<DataInitializationRunEntity>();
        init.ToTable("data_initialization_runs");
        init.HasKey(x => x.Id);
        init.Property(x => x.Engine).HasConversion<string>().HasMaxLength(20);
        init.Property(x => x.DatabaseName).HasMaxLength(100).IsRequired();
        init.Property(x => x.SeedVersion).HasMaxLength(50).IsRequired();
        init.Property(x => x.State).HasConversion<string>().HasMaxLength(20);
        init.HasIndex(x => new { x.Engine, x.DatabaseName, x.SeedVersion }).IsUnique();
    }

    private static void ConfigureRagDocuments(ModelBuilder modelBuilder)
    {
        var document = modelBuilder.Entity<RagDocumentEntity>();
        document.ToTable("rag_documents");
        document.HasKey(x => x.Id);
        document.Property(x => x.DocumentType).HasConversion<string>().HasMaxLength(20);
        document.Property(x => x.Engine).HasMaxLength(20).IsRequired();
        document.Property(x => x.Vendor).HasMaxLength(40).IsRequired();
        document.Property(x => x.Topic).HasMaxLength(40).IsRequired();
        document.Property(x => x.SourcePath).HasMaxLength(300).IsRequired();
        document.Property(x => x.SourceTitle).HasMaxLength(300).IsRequired();
        document.Property(x => x.ContentHash).HasMaxLength(128).IsRequired();
        document.HasIndex(x => x.SourcePath)
            .IsUnique()
            .HasDatabaseName("idx_rag_documents_source_path");
        document.HasIndex(x => new { x.Engine, x.Vendor, x.Topic })
            .HasDatabaseName("idx_rag_documents_engine_vendor_topic");
    }

    private void ConfigureRagDocumentChunks(ModelBuilder modelBuilder)
    {
        var chunk = modelBuilder.Entity<RagDocumentChunkEntity>();
        chunk.ToTable("rag_document_chunks");
        chunk.HasKey(x => x.Id);
        chunk.Property(x => x.ChunkKey).HasMaxLength(200).IsRequired();
        chunk.Property(x => x.Title).HasMaxLength(300).IsRequired();
        chunk.Property(x => x.SectionPath).HasMaxLength(500).IsRequired();
        chunk.Property(x => x.ProductVersion).HasMaxLength(100);
        chunk.Property(x => x.AppliesTo).HasMaxLength(200);
        if (Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
        {
            chunk.Ignore(x => x.Embedding);
        }
        else
        {
            chunk.Property(x => x.Embedding).HasColumnType("vector");
        }
        chunk.HasIndex(x => new { x.DocumentId, x.ChunkKey })
            .IsUnique()
            .HasDatabaseName("idx_rag_document_chunks_document_chunk_key");
        chunk.HasOne(x => x.Document)
            .WithMany(x => x.Chunks)
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureRagCaseRecords(ModelBuilder modelBuilder)
    {
        var caseRecord = modelBuilder.Entity<RagCaseRecordEntity>();
        caseRecord.ToTable("rag_case_records");
        caseRecord.HasKey(x => x.Id);
        caseRecord.Property(x => x.Engine).HasMaxLength(20).IsRequired();
        caseRecord.Property(x => x.ProblemType).HasMaxLength(80).IsRequired();
        caseRecord.Property(x => x.Outcome).HasMaxLength(40).IsRequired();
        caseRecord.Property(x => x.ReviewStatus).HasMaxLength(40).IsRequired();
        caseRecord.Property(x => x.RecommendationType).HasMaxLength(60).IsRequired();
        caseRecord.HasIndex(x => x.WorkflowSessionId)
            .IsUnique()
            .HasDatabaseName("idx_rag_case_records_workflow_session");
        caseRecord.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.RagCaseRecords)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureRagCaseEvidenceLinks(ModelBuilder modelBuilder)
    {
        var link = modelBuilder.Entity<RagCaseEvidenceLinkEntity>();
        link.ToTable("rag_case_evidence_links");
        link.HasKey(x => x.Id);
        link.Property(x => x.EvidenceReference).HasMaxLength(200).IsRequired();
        link.Property(x => x.RecommendationKey).HasMaxLength(200).IsRequired();
        link.HasIndex(x => x.CaseRecordId)
            .HasDatabaseName("idx_rag_case_evidence_links_case_record");
        link.HasOne(x => x.CaseRecord)
            .WithMany(x => x.EvidenceLinks)
            .HasForeignKey(x => x.CaseRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureRagRetrievalSnapshots(ModelBuilder modelBuilder)
    {
        var snapshot = modelBuilder.Entity<RagRetrievalSnapshotEntity>();
        snapshot.ToTable("rag_retrieval_snapshots");
        snapshot.HasKey(x => x.Id);
        snapshot.HasIndex(x => x.WorkflowSessionId)
            .HasDatabaseName("idx_rag_retrieval_snapshots_session");
        snapshot.HasIndex(x => x.NodeExecutionId)
            .HasDatabaseName("idx_rag_retrieval_snapshots_node");
        snapshot.HasOne(x => x.WorkflowSession)
            .WithMany(x => x.RagRetrievalSnapshots)
            .HasForeignKey(x => x.WorkflowSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        snapshot.HasOne(x => x.NodeExecution)
            .WithMany(x => x.RagRetrievalSnapshots)
            .HasForeignKey(x => x.NodeExecutionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

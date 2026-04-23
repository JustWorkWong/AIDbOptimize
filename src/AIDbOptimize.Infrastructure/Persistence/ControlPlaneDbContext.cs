using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Seed.Enums;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// 控制面数据库上下文。
/// 负责保存 MCP 连接、工具配置、执行记录和初始化状态。
/// </summary>
public sealed class ControlPlaneDbContext(DbContextOptions<ControlPlaneDbContext> options) : DbContext(options)
{
    /// <summary>
    /// MCP 连接配置集合。
    /// </summary>
    public DbSet<McpConnectionEntity> McpConnections => Set<McpConnectionEntity>();

    /// <summary>
    /// MCP 工具配置集合。
    /// </summary>
    public DbSet<McpToolEntity> McpTools => Set<McpToolEntity>();

    /// <summary>
    /// MCP 工具执行记录集合。
    /// </summary>
    public DbSet<McpToolExecutionEntity> McpToolExecutions => Set<McpToolExecutionEntity>();

    /// <summary>
    /// 数据初始化运行状态集合。
    /// </summary>
    public DbSet<DataInitializationRunEntity> DataInitializationRuns => Set<DataInitializationRunEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureMcpConnections(modelBuilder);
        ConfigureMcpTools(modelBuilder);
        ConfigureMcpToolExecutions(modelBuilder);
        ConfigureDataInitializationRuns(modelBuilder);
    }

    /// <summary>
    /// 配置 MCP 连接表。
    /// </summary>
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

    /// <summary>
    /// 配置 MCP 工具表。
    /// </summary>
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

    /// <summary>
    /// 配置 MCP 工具执行记录表。
    /// </summary>
    private static void ConfigureMcpToolExecutions(ModelBuilder modelBuilder)
    {
        var execution = modelBuilder.Entity<McpToolExecutionEntity>();
        execution.ToTable("mcp_tool_executions");
        execution.HasKey(x => x.Id);
        execution.Property(x => x.RequestedBy).HasMaxLength(100).IsRequired();
        execution.Property(x => x.Status).HasMaxLength(20).IsRequired();
        execution.HasOne(x => x.Connection)
            .WithMany()
            .HasForeignKey(x => x.ConnectionId)
            .OnDelete(DeleteBehavior.Restrict);
        execution.HasOne(x => x.Tool)
            .WithMany(x => x.Executions)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// 配置数据初始化状态表。
    /// </summary>
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
}

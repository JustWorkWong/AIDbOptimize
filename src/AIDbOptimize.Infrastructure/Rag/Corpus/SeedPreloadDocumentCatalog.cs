namespace AIDbOptimize.Infrastructure.Rag.Corpus;

public static class SeedPreloadDocumentCatalog
{
    private static readonly IReadOnlyList<RagSourceDocument> Documents =
    [
        CreateFactDocument(
            engine: "mysql",
            bucket: "mysql",
            vendor: "mysql",
            topic: "memory",
            shortTitle: "server-system-variables",
            sourceTitle: "MySQL 8.4 Reference Manual - Server System Variables",
            sourceUrl: "https://dev.mysql.com/doc/refman/8.4/en/server-system-variables.html",
            seedId: "mysql-refman-server-system-variables"),
        CreateFactDocument(
            engine: "postgresql",
            bucket: "postgresql",
            vendor: "postgresql",
            topic: "memory",
            shortTitle: "runtime-config-resource",
            sourceTitle: "PostgreSQL Runtime Configuration - Resource Consumption",
            sourceUrl: "https://www.postgresql.org/docs/current/runtime-config-resource.html",
            seedId: "postgresql-runtime-config-resource"),
        CreateFactDocument(
            engine: "mysql",
            bucket: "cloud",
            vendor: "aliyun",
            topic: "memory",
            shortTitle: "rds-mysql-parameter-tuning",
            sourceTitle: "阿里云 RDS MySQL 参数调优",
            sourceUrl: "https://help.aliyun.com/zh/rds/apsaradb-rds-for-mysql/use-parameters-to-tune-an-apsaradb-rds-for-mysql-instance",
            seedId: "aliyun-rds-mysql-parameter-tuning"),
        CreateFactDocument(
            engine: "mysql",
            bucket: "cloud",
            vendor: "aws",
            topic: "memory",
            shortTitle: "appendix-mysql-parameters",
            sourceTitle: "AWS RDS MySQL Parameters",
            sourceUrl: "https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html",
            seedId: "aws-rds-mysql-parameters"),
        CreateFactDocument(
            engine: "mysql",
            bucket: "cloud",
            vendor: "azure",
            topic: "connections",
            shortTitle: "flexible-server-parameters",
            sourceTitle: "Azure Database for MySQL Flexible Server Parameters",
            sourceUrl: "https://learn.microsoft.com/azure/mysql/flexible-server/concepts-server-parameters",
            seedId: "azure-mysql-flexible-server-parameters"),
        CreateFactDocument(
            engine: "mysql",
            bucket: "cloud",
            vendor: "gcp",
            topic: "connections",
            shortTitle: "cloud-sql-flags",
            sourceTitle: "Cloud SQL for MySQL Flags",
            sourceUrl: "https://cloud.google.com/sql/docs/mysql/flags",
            seedId: "gcp-cloud-sql-mysql-flags")
    ];

    public static IReadOnlyList<RagSourceDocument> GetDocuments()
    {
        return Documents;
    }

    private static RagSourceDocument CreateFactDocument(
        string engine,
        string bucket,
        string vendor,
        string topic,
        string shortTitle,
        string sourceTitle,
        string sourceUrl,
        string seedId)
    {
        var fileName = RagCorpusFileNamer.CreateFactFileName(engine, vendor, topic, shortTitle);
        var relativePath = $"facts/{bucket}/{fileName}";
        var parsed = RagCorpusFileNamer.ParseRelativePath(relativePath);

        return parsed with
        {
            SourceTitle = sourceTitle,
            SourceUrl = sourceUrl,
            Metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["seed_id"] = seedId,
                ["engine"] = parsed.Engine,
                ["vendor"] = parsed.Vendor ?? string.Empty,
                ["topic"] = parsed.Topic ?? string.Empty,
                ["bucket"] = bucket
            }
        };
    }
}

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.PostgreSqlLab
{
    /// <summary>
    /// PostgreSQL 业务测试库 10w 订单种子迁移。
    /// 通过迁移执行 SQL 造数，幂等由 EF Core migration history 保证。
    /// </summary>
    [DbContext(typeof(PostgreSqlLabDbContext))]
    [Migration("20260424000100_SeedPostgreSqlLab10wOrders")]
    public partial class SeedPostgreSqlLab10wOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO orders ("OrderNumber", "CustomerId", "Status", "TotalAmount", "CreatedAt", "UpdatedAt")
                SELECT
                    'ORD-PG-' || LPAD(gs::text, 10, '0') AS "OrderNumber",
                    ((gs - 1) % 50000) + 1 AS "CustomerId",
                    CASE (gs % 5)
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Paid'
                        WHEN 2 THEN 'Shipped'
                        WHEN 3 THEN 'Completed'
                        ELSE 'Cancelled'
                    END AS "Status",
                    0::numeric(18,2) AS "TotalAmount",
                    NOW() - ((gs % 365) || ' days')::interval AS "CreatedAt",
                    NOW() AS "UpdatedAt"
                FROM generate_series(1, 100000) AS gs;
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO order_items ("OrderId", "ProductId", "ProductName", "Quantity", "UnitPrice", "LineAmount", "CreatedAt")
                SELECT
                    o."Id" AS "OrderId",
                    ((o."Id" - 1) % 1000) + 1 AS "ProductId",
                    'Product-' || (((o."Id" - 1) % 1000) + 1) AS "ProductName",
                    ((o."Id" - 1) % 5) + 1 AS "Quantity",
                    ROUND((((o."Id" - 1) % 200) + 1) * 1.5, 2) AS "UnitPrice",
                    ROUND(((((o."Id" - 1) % 5) + 1) * ((((o."Id" - 1) % 200) + 1) * 1.5))::numeric, 2) AS "LineAmount",
                    o."CreatedAt" AS "CreatedAt"
                FROM orders o;
                """);

            migrationBuilder.Sql(
                """
                UPDATE orders o
                SET "TotalAmount" = totals.total_amount,
                    "UpdatedAt" = NOW()
                FROM (
                    SELECT "OrderId", ROUND(SUM("LineAmount"), 2) AS total_amount
                    FROM order_items
                    GROUP BY "OrderId"
                ) totals
                WHERE o."Id" = totals."OrderId";
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""DELETE FROM order_items;""");
            migrationBuilder.Sql("""DELETE FROM orders;""");
        }
    }
}

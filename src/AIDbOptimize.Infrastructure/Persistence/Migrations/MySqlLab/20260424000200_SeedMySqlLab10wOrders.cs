using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDbOptimize.Infrastructure.Persistence.Migrations.MySqlLab
{
    /// <summary>
    /// MySQL 业务测试库 10w 订单种子迁移。
    /// 通过迁移执行 SQL 造数，幂等由 EF Core migration history 保证。
    /// </summary>
    [DbContext(typeof(MySqlLabDbContext))]
    [Migration("20260424000200_SeedMySqlLab10wOrders")]
    public partial class SeedMySqlLab10wOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO orders (`OrderNumber`, `CustomerId`, `Status`, `TotalAmount`, `CreatedAt`, `UpdatedAt`)
                SELECT
                    CONCAT('ORD-MY-', LPAD(seq_num, 10, '0')) AS `OrderNumber`,
                    MOD(seq_num - 1, 50000) + 1 AS `CustomerId`,
                    ELT(MOD(seq_num, 5) + 1, 'Pending', 'Paid', 'Shipped', 'Completed', 'Cancelled') AS `Status`,
                    0.00 AS `TotalAmount`,
                    DATE_SUB(UTC_TIMESTAMP(), INTERVAL MOD(seq_num, 365) DAY) AS `CreatedAt`,
                    UTC_TIMESTAMP() AS `UpdatedAt`
                FROM (
                    SELECT ones.n + tens.n * 10 + hundreds.n * 100 + thousands.n * 1000 + tenthousands.n * 10000 + 1 AS seq_num
                    FROM
                        (SELECT 0 AS n UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) ones
                    CROSS JOIN
                        (SELECT 0 AS n UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) tens
                    CROSS JOIN
                        (SELECT 0 AS n UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) hundreds
                    CROSS JOIN
                        (SELECT 0 AS n UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) thousands
                    CROSS JOIN
                        (SELECT 0 AS n UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) tenthousands
                ) numbers
                WHERE seq_num <= 100000;
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO order_items (`OrderId`, `ProductId`, `ProductName`, `Quantity`, `UnitPrice`, `LineAmount`, `CreatedAt`)
                SELECT
                    o.`Id` AS `OrderId`,
                    MOD(o.`Id` - 1, 1000) + 1 AS `ProductId`,
                    CONCAT('Product-', MOD(o.`Id` - 1, 1000) + 1) AS `ProductName`,
                    MOD(o.`Id` - 1, 5) + 1 AS `Quantity`,
                    ROUND((MOD(o.`Id` - 1, 200) + 1) * 1.5, 2) AS `UnitPrice`,
                    ROUND((MOD(o.`Id` - 1, 5) + 1) * ((MOD(o.`Id` - 1, 200) + 1) * 1.5), 2) AS `LineAmount`,
                    o.`CreatedAt` AS `CreatedAt`
                FROM orders o;
                """);

            migrationBuilder.Sql(
                """
                UPDATE orders o
                JOIN (
                    SELECT `OrderId`, ROUND(SUM(`LineAmount`), 2) AS total_amount
                    FROM order_items
                    GROUP BY `OrderId`
                ) totals ON o.`Id` = totals.`OrderId`
                SET o.`TotalAmount` = totals.total_amount,
                    o.`UpdatedAt` = UTC_TIMESTAMP();
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""DELETE FROM order_items;""");
            migrationBuilder.Sql("""DELETE FROM orders;""");
        }
    }
}

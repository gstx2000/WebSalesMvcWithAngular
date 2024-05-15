using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class SumProfitSalesRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Margin",
                table: "SalesRecord",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Profit",
                table: "SalesRecord",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Margin",
                table: "SalesRecord");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "SalesRecord");
        }
    }
}

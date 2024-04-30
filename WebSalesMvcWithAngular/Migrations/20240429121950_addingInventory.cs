using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class addingInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AcquisitionCost",
                table: "Product",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InventoryCost",
                table: "Product",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InventoryQuantity",
                table: "Product",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "InventoryUnitMeas",
                table: "Product",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "MinimumInventoryQuantity",
                table: "Product",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionCost",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "InventoryCost",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "InventoryQuantity",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "InventoryUnitMeas",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MinimumInventoryQuantity",
                table: "Product");
        }
    }
}

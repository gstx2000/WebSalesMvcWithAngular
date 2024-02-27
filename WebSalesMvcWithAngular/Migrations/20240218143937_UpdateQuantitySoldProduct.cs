using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuantitySoldProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SoldProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SoldProduct");
        }
    }
}

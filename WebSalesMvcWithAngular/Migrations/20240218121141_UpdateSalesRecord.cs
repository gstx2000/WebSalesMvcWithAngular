using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentForm",
                table: "SalesRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SoldProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SalesRecordId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldProduct_SalesRecord_SalesRecordId",
                        column: x => x.SalesRecordId,
                        principalTable: "SalesRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProduct_ProductId",
                table: "SoldProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProduct_SalesRecordId",
                table: "SoldProduct",
                column: "SalesRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldProduct");

            migrationBuilder.DropColumn(
                name: "PaymentForm",
                table: "SalesRecord");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSPToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldProduct_Product_ProductId",
                table: "SoldProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProduct_SalesRecord_SalesRecordId",
                table: "SoldProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldProduct",
                table: "SoldProduct");

            migrationBuilder.RenameTable(
                name: "SoldProduct",
                newName: "SoldProducts");

            migrationBuilder.RenameIndex(
                name: "IX_SoldProduct_SalesRecordId",
                table: "SoldProducts",
                newName: "IX_SoldProducts_SalesRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldProduct_ProductId",
                table: "SoldProducts",
                newName: "IX_SoldProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldProducts",
                table: "SoldProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_Product_ProductId",
                table: "SoldProducts",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_SalesRecord_SalesRecordId",
                table: "SoldProducts",
                column: "SalesRecordId",
                principalTable: "SalesRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_Product_ProductId",
                table: "SoldProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_SalesRecord_SalesRecordId",
                table: "SoldProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldProducts",
                table: "SoldProducts");

            migrationBuilder.RenameTable(
                name: "SoldProducts",
                newName: "SoldProduct");

            migrationBuilder.RenameIndex(
                name: "IX_SoldProducts_SalesRecordId",
                table: "SoldProduct",
                newName: "IX_SoldProduct_SalesRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldProducts_ProductId",
                table: "SoldProduct",
                newName: "IX_SoldProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldProduct",
                table: "SoldProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProduct_Product_ProductId",
                table: "SoldProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProduct_SalesRecord_SalesRecordId",
                table: "SoldProduct",
                column: "SalesRecordId",
                principalTable: "SalesRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

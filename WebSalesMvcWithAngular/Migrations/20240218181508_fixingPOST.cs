using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class fixingPOST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_SalesRecord_SalesRecordId",
                table: "SoldProducts");

            migrationBuilder.AlterColumn<int>(
                name: "SalesRecordId",
                table: "SoldProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_SalesRecord_SalesRecordId",
                table: "SoldProducts",
                column: "SalesRecordId",
                principalTable: "SalesRecord",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldProducts_SalesRecord_SalesRecordId",
                table: "SoldProducts");

            migrationBuilder.AlterColumn<int>(
                name: "SalesRecordId",
                table: "SoldProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldProducts_SalesRecord_SalesRecordId",
                table: "SoldProducts",
                column: "SalesRecordId",
                principalTable: "SalesRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

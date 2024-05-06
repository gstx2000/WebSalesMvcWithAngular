using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class AdjustingSupplierAdresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_Adress_AdressId",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_AdressId",
                table: "Supplier");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierType",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AdressId",
                table: "Supplier",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Adress",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Adress",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CNPJ = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CPF = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RG = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Adress_CustomerId",
                table: "Adress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Adress_SupplierId",
                table: "Adress",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adress_Customer_CustomerId",
                table: "Adress",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adress_Supplier_SupplierId",
                table: "Adress",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adress_Customer_CustomerId",
                table: "Adress");

            migrationBuilder.DropForeignKey(
                name: "FK_Adress_Supplier_SupplierId",
                table: "Adress");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Adress_CustomerId",
                table: "Adress");

            migrationBuilder.DropIndex(
                name: "IX_Adress_SupplierId",
                table: "Adress");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Adress");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Adress");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierType",
                table: "Supplier",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "AdressId",
                table: "Supplier",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_AdressId",
                table: "Supplier",
                column: "AdressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_Adress_AdressId",
                table: "Supplier",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "AdressId");
        }
    }
}

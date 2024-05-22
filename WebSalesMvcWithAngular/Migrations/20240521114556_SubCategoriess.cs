using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class SubCategoriess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_SubCategoryId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "Category",
                newName: "ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_SubCategoryId",
                table: "Category",
                newName: "IX_Category_ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentCategoryId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryId",
                table: "Category",
                newName: "SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                newName: "IX_Category_SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_SubCategoryId",
                table: "Category",
                column: "SubCategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}

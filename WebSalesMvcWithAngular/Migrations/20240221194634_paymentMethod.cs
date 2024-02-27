using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSalesMvcWithAngular.Migrations
{
    /// <inheritdoc />
    public partial class paymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentForm",
                table: "SalesRecord",
                newName: "PaymentMethod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "SalesRecord",
                newName: "PaymentForm");
        }
    }
}

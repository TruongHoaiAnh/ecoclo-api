using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderV02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "orderDetails");

            migrationBuilder.AddColumn<float>(
                name: "OrderTotal",
                table: "orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "orders");

            migrationBuilder.AddColumn<float>(
                name: "OrderTotal",
                table: "orderDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}

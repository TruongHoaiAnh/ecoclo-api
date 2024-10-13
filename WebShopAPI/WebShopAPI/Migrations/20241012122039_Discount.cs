using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class Discount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "ShippingFee",
                table: "orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ShippingFee",
                table: "orders");
        }
    }
}

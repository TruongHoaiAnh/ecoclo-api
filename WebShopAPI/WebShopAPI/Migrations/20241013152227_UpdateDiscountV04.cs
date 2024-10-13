using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscountV04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsageLimit",
                table: "discounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsageLimit",
                table: "discounts",
                type: "int",
                nullable: true);
        }
    }
}

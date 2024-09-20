using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class SwapPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductItem",
                table: "productItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "productItems");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "productItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductItem",
                table: "productItems",
                column: "IdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductItem",
                table: "productItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "products");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "productItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "productItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductItem",
                table: "productItems",
                column: "IdPro",
                principalTable: "products",
                principalColumn: "IdPro");
        }
    }
}

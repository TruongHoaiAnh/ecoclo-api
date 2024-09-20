using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productItems_products_productIdPro",
                table: "productItems");

            migrationBuilder.DropIndex(
                name: "IX_productItems_productIdPro",
                table: "productItems");

            migrationBuilder.DropColumn(
                name: "productIdPro",
                table: "productItems");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "productItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_productItems_IdPro",
                table: "productItems",
                column: "IdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductItem",
                table: "productItems",
                column: "IdPro",
                principalTable: "products",
                principalColumn: "IdPro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductItem",
                table: "productItems");

            migrationBuilder.DropIndex(
                name: "IX_productItems_IdPro",
                table: "productItems");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "productItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "productIdPro",
                table: "productItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_productItems_productIdPro",
                table: "productItems",
                column: "productIdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_productItems_products_productIdPro",
                table: "productItems",
                column: "productIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImgv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_imgPros_products_productIdPro",
                table: "imgPros");

            migrationBuilder.DropIndex(
                name: "IX_imgPros_productIdPro",
                table: "imgPros");

            migrationBuilder.DropColumn(
                name: "productIdPro",
                table: "imgPros");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "imgPros",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_imgPros_IdPro",
                table: "imgPros",
                column: "IdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_ImgPro_Product",
                table: "imgPros",
                column: "IdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImgPro_Product",
                table: "imgPros");

            migrationBuilder.DropIndex(
                name: "IX_imgPros_IdPro",
                table: "imgPros");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "imgPros",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "productIdPro",
                table: "imgPros",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_imgPros_productIdPro",
                table: "imgPros",
                column: "productIdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_imgPros_products_productIdPro",
                table: "imgPros",
                column: "productIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

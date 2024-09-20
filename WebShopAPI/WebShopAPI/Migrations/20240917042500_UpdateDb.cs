using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_imgPros_products_IdProNavigationIdPro",
                table: "imgPros");

            migrationBuilder.DropForeignKey(
                name: "FK_productItems_products_IdProNavigationIdPro",
                table: "productItems");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_Users_IdAccNavigationId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_products_IdProNavigationIdPro",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "IdProNavigationIdPro",
                table: "reviews",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "IdAccNavigationId",
                table: "reviews",
                newName: "productIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_IdProNavigationIdPro",
                table: "reviews",
                newName: "IX_reviews_userId");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_IdAccNavigationId",
                table: "reviews",
                newName: "IX_reviews_productIdPro");

            migrationBuilder.RenameColumn(
                name: "IdProNavigationIdPro",
                table: "productItems",
                newName: "productIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_productItems_IdProNavigationIdPro",
                table: "productItems",
                newName: "IX_productItems_productIdPro");

            migrationBuilder.RenameColumn(
                name: "IdProNavigationIdPro",
                table: "imgPros",
                newName: "productIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_imgPros_IdProNavigationIdPro",
                table: "imgPros",
                newName: "IX_imgPros_productIdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_imgPros_products_productIdPro",
                table: "imgPros",
                column: "productIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productItems_products_productIdPro",
                table: "productItems",
                column: "productIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_Users_userId",
                table: "reviews",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_products_productIdPro",
                table: "reviews",
                column: "productIdPro",
                principalTable: "products",
                principalColumn: "IdPro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_imgPros_products_productIdPro",
                table: "imgPros");

            migrationBuilder.DropForeignKey(
                name: "FK_productItems_products_productIdPro",
                table: "productItems");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_Users_userId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_products_productIdPro",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "reviews",
                newName: "IdProNavigationIdPro");

            migrationBuilder.RenameColumn(
                name: "productIdPro",
                table: "reviews",
                newName: "IdAccNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_userId",
                table: "reviews",
                newName: "IX_reviews_IdProNavigationIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_productIdPro",
                table: "reviews",
                newName: "IX_reviews_IdAccNavigationId");

            migrationBuilder.RenameColumn(
                name: "productIdPro",
                table: "productItems",
                newName: "IdProNavigationIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_productItems_productIdPro",
                table: "productItems",
                newName: "IX_productItems_IdProNavigationIdPro");

            migrationBuilder.RenameColumn(
                name: "productIdPro",
                table: "imgPros",
                newName: "IdProNavigationIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_imgPros_productIdPro",
                table: "imgPros",
                newName: "IX_imgPros_IdProNavigationIdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_imgPros_products_IdProNavigationIdPro",
                table: "imgPros",
                column: "IdProNavigationIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productItems_products_IdProNavigationIdPro",
                table: "productItems",
                column: "IdProNavigationIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_Users_IdAccNavigationId",
                table: "reviews",
                column: "IdAccNavigationId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_products_IdProNavigationIdPro",
                table: "reviews",
                column: "IdProNavigationIdPro",
                principalTable: "products",
                principalColumn: "IdPro");
        }
    }
}

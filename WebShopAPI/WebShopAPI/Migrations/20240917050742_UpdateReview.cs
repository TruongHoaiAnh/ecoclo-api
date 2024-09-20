using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_Users_userId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_products_productIdPro",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_productIdPro",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_userId",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "productIdPro",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "reviews");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdAcc",
                table: "reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_IdAcc",
                table: "reviews",
                column: "IdAcc");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_IdPro",
                table: "reviews",
                column: "IdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Product",
                table: "reviews",
                column: "IdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User",
                table: "reviews",
                column: "IdAcc",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Product",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_IdAcc",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_IdPro",
                table: "reviews");

            migrationBuilder.AlterColumn<string>(
                name: "IdPro",
                table: "reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdAcc",
                table: "reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "productIdPro",
                table: "reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_productIdPro",
                table: "reviews",
                column: "productIdPro");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_userId",
                table: "reviews",
                column: "userId");

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
    }
}

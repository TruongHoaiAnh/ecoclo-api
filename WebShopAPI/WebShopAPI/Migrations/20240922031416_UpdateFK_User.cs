using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFK_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wishlist_Users",
                table: "wishlists");

            migrationBuilder.AlterColumn<string>(
                name: "IdAcc",
                table: "wishlists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_wishlists_IdAcc",
                table: "wishlists",
                column: "IdAcc");

            migrationBuilder.AddForeignKey(
                name: "FK_wishlist_Users",
                table: "wishlists",
                column: "IdAcc",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wishlist_Users",
                table: "wishlists");

            migrationBuilder.DropIndex(
                name: "IX_wishlists_IdAcc",
                table: "wishlists");

            migrationBuilder.AlterColumn<string>(
                name: "IdAcc",
                table: "wishlists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_wishlist_Users",
                table: "wishlists",
                column: "IdPro",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

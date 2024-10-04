using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shopping_cart_user",
                table: "shoppingCarts");

            migrationBuilder.AlterColumn<string>(
                name: "IdAcc",
                table: "shoppingCarts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_IdAcc",
                table: "shoppingCarts",
                column: "IdAcc");

            migrationBuilder.AddForeignKey(
                name: "FK_shopping_cart_user",
                table: "shoppingCarts",
                column: "IdAcc",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shopping_cart_user",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_IdAcc",
                table: "shoppingCarts");

            migrationBuilder.AlterColumn<string>(
                name: "IdAcc",
                table: "shoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_shopping_cart_user",
                table: "shoppingCarts",
                column: "IdCart",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

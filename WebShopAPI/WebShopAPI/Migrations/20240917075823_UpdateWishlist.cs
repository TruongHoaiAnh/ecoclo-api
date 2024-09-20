using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wishlists",
                columns: table => new
                {
                    IdWishlist = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlists", x => x.IdWishlist);
                    table.ForeignKey(
                        name: "FK_wishlist_Users",
                        column: x => x.IdPro,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_wishlist_product",
                        column: x => x.IdPro,
                        principalTable: "products",
                        principalColumn: "IdPro");
                });

            migrationBuilder.CreateIndex(
                name: "IX_wishlists_IdPro",
                table: "wishlists",
                column: "IdPro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wishlists");
        }
    }
}

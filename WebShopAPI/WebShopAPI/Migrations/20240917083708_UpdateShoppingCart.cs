using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdCate",
                table: "products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    IdCate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameCate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.IdCate);
                });

            migrationBuilder.CreateTable(
                name: "shoppingCarts",
                columns: table => new
                {
                    IdCart = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoppingCarts", x => x.IdCart);
                    table.ForeignKey(
                        name: "FK_shopping_cart_user",
                        column: x => x.IdCart,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "shoppingCartItems",
                columns: table => new
                {
                    IdCartItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCart = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true),
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoppingCartItems", x => x.IdCartItem);
                    table.ForeignKey(
                        name: "FK_shopping_cart_item_product",
                        column: x => x.IdPro,
                        principalTable: "products",
                        principalColumn: "IdPro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shopping_cart_item_product_item",
                        column: x => x.IdProItem,
                        principalTable: "productItems",
                        principalColumn: "IdProItem");
                    table.ForeignKey(
                        name: "FK_shopping_cart_item_shopping_cart",
                        column: x => x.IdCart,
                        principalTable: "shoppingCarts",
                        principalColumn: "IdCart");
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_IdCate",
                table: "products",
                column: "IdCate");

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCartItems_IdCart",
                table: "shoppingCartItems",
                column: "IdCart");

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCartItems_IdPro",
                table: "shoppingCartItems",
                column: "IdPro");

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCartItems_IdProItem",
                table: "shoppingCartItems",
                column: "IdProItem");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category",
                table: "products",
                column: "IdCate",
                principalTable: "categories",
                principalColumn: "IdCate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category",
                table: "products");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "shoppingCartItems");

            migrationBuilder.DropTable(
                name: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_products_IdCate",
                table: "products");

            migrationBuilder.DropColumn(
                name: "IdCate",
                table: "products");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banners",
                columns: table => new
                {
                    IdBanner = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LinkImg = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banners", x => x.IdBanner);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    IdCate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameCate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusCate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.IdCate);
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    IdDiscount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<float>(type: "real", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MinimumOrderAmount = table.Column<float>(type: "real", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.IdDiscount);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Avt = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThumbnailImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BestSeller = table.Column<int>(type: "int", nullable: true),
                    StatusProduct = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.IdPro);
                    table.ForeignKey(
                        name: "FK_Product_Category",
                        column: x => x.IdCate,
                        principalTable: "categories",
                        principalColumn: "IdCate");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    IdOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    OrderStart = table.Column<int>(type: "int", nullable: true),
                    OrderInProgress = table.Column<int>(type: "int", nullable: true),
                    OrderEnd = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingFee = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_order_user",
                        column: x => x.IdAcc,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "shoppingCarts",
                columns: table => new
                {
                    IdCart = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoppingCarts", x => x.IdCart);
                    table.ForeignKey(
                        name: "FK_shopping_cart_user",
                        column: x => x.IdAcc,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "imgPros",
                columns: table => new
                {
                    IdImg = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LinkImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imgPros", x => x.IdImg);
                    table.ForeignKey(
                        name: "FK_ImgPro_Product",
                        column: x => x.IdPro,
                        principalTable: "products",
                        principalColumn: "IdPro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productItems",
                columns: table => new
                {
                    IdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StatusProItem = table.Column<int>(type: "int", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productItems", x => x.IdProItem);
                    table.ForeignKey(
                        name: "FK_Product_ProductItem",
                        column: x => x.IdPro,
                        principalTable: "products",
                        principalColumn: "IdPro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    IdReview = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: true),
                    Dislike = table.Column<int>(type: "int", nullable: true),
                    statusReview = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.IdReview);
                    table.ForeignKey(
                        name: "FK_Review_Product",
                        column: x => x.IdPro,
                        principalTable: "products",
                        principalColumn: "IdPro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_User",
                        column: x => x.IdAcc,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wishlists",
                columns: table => new
                {
                    IdWishlist = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlists", x => x.IdWishlist);
                    table.ForeignKey(
                        name: "FK_wishlist_Users",
                        column: x => x.IdAcc,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_wishlist_product",
                        column: x => x.IdPro,
                        principalTable: "products",
                        principalColumn: "IdPro");
                });

            migrationBuilder.CreateTable(
                name: "orderDetails",
                columns: table => new
                {
                    IdOrderDetail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    OrderTotal = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<float>(type: "real", nullable: true),
                    OrderIdOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductItemIdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderDetails", x => x.IdOrderDetail);
                    table.ForeignKey(
                        name: "FK_orderDetails_orders_OrderIdOrder",
                        column: x => x.OrderIdOrder,
                        principalTable: "orders",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderDetails_productItems_ProductItemIdProItem",
                        column: x => x.ProductItemIdProItem,
                        principalTable: "productItems",
                        principalColumn: "IdProItem",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_detail_order",
                        column: x => x.IdOrder,
                        principalTable: "orders",
                        principalColumn: "IdOrder");
                    table.ForeignKey(
                        name: "FK_order_detail_product_item",
                        column: x => x.IdProItem,
                        principalTable: "productItems",
                        principalColumn: "IdProItem");
                });

            migrationBuilder.CreateTable(
                name: "shoppingCartItems",
                columns: table => new
                {
                    IdCartItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCart = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
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
                name: "IX_imgPros_IdPro",
                table: "imgPros",
                column: "IdPro");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_IdOrder",
                table: "orderDetails",
                column: "IdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_IdProItem",
                table: "orderDetails",
                column: "IdProItem");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_OrderIdOrder",
                table: "orderDetails",
                column: "OrderIdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_ProductItemIdProItem",
                table: "orderDetails",
                column: "ProductItemIdProItem");

            migrationBuilder.CreateIndex(
                name: "IX_orders_IdAcc",
                table: "orders",
                column: "IdAcc");

            migrationBuilder.CreateIndex(
                name: "IX_productItems_IdPro",
                table: "productItems",
                column: "IdPro");

            migrationBuilder.CreateIndex(
                name: "IX_products_IdCate",
                table: "products",
                column: "IdCate");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_IdAcc",
                table: "reviews",
                column: "IdAcc");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_IdPro",
                table: "reviews",
                column: "IdPro");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_IdAcc",
                table: "shoppingCarts",
                column: "IdAcc");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_wishlists_IdAcc",
                table: "wishlists",
                column: "IdAcc");

            migrationBuilder.CreateIndex(
                name: "IX_wishlists_IdPro",
                table: "wishlists",
                column: "IdPro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banners");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "imgPros");

            migrationBuilder.DropTable(
                name: "orderDetails");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "shoppingCartItems");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "wishlists");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "productItems");

            migrationBuilder.DropTable(
                name: "shoppingCarts");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}

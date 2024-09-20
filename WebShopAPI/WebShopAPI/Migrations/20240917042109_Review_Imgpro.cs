using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class Review_Imgpro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productItems_products_productIdPro",
                table: "productItems");

            migrationBuilder.RenameColumn(
                name: "productIdPro",
                table: "productItems",
                newName: "IdProNavigationIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_productItems_productIdPro",
                table: "productItems",
                newName: "IX_productItems_IdProNavigationIdPro");

            migrationBuilder.CreateTable(
                name: "imgPros",
                columns: table => new
                {
                    IdImg = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProNavigationIdPro = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imgPros", x => x.IdImg);
                    table.ForeignKey(
                        name: "FK_imgPros_products_IdProNavigationIdPro",
                        column: x => x.IdProNavigationIdPro,
                        principalTable: "products",
                        principalColumn: "IdPro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    IdReview = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatingValue = table.Column<double>(type: "float", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdAccNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdProNavigationIdPro = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.IdReview);
                    table.ForeignKey(
                        name: "FK_reviews_Users_IdAccNavigationId",
                        column: x => x.IdAccNavigationId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_reviews_products_IdProNavigationIdPro",
                        column: x => x.IdProNavigationIdPro,
                        principalTable: "products",
                        principalColumn: "IdPro");
                });

            migrationBuilder.CreateIndex(
                name: "IX_imgPros_IdProNavigationIdPro",
                table: "imgPros",
                column: "IdProNavigationIdPro");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_IdAccNavigationId",
                table: "reviews",
                column: "IdAccNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_IdProNavigationIdPro",
                table: "reviews",
                column: "IdProNavigationIdPro");

            migrationBuilder.AddForeignKey(
                name: "FK_productItems_products_IdProNavigationIdPro",
                table: "productItems",
                column: "IdProNavigationIdPro",
                principalTable: "products",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productItems_products_IdProNavigationIdPro",
                table: "productItems");

            migrationBuilder.DropTable(
                name: "imgPros");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.RenameColumn(
                name: "IdProNavigationIdPro",
                table: "productItems",
                newName: "productIdPro");

            migrationBuilder.RenameIndex(
                name: "IX_productItems_IdProNavigationIdPro",
                table: "productItems",
                newName: "IX_productItems_productIdPro");

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

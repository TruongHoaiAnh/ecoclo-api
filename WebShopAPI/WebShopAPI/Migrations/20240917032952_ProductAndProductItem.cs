using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductAndProductItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    IdPro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BestSeller = table.Column<int>(type: "int", nullable: false),
                    StatusProduct = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.IdPro);
                });

            migrationBuilder.CreateTable(
                name: "productItems",
                columns: table => new
                {
                    IdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    StatusProItem = table.Column<int>(type: "int", nullable: false),
                    IdPro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productIdPro = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productItems", x => x.IdProItem);
                    table.ForeignKey(
                        name: "FK_productItems_products_productIdPro",
                        column: x => x.productIdPro,
                        principalTable: "products",
                        principalColumn: "IdPro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productItems_productIdPro",
                table: "productItems",
                column: "productIdPro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productItems");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}

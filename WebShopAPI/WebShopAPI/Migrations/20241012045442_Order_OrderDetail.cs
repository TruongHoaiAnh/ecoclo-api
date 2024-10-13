using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class Order_OrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    IdOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAcc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethodId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    OrderStart = table.Column<int>(type: "int", nullable: true),
                    OrderInProgress = table.Column<int>(type: "int", nullable: true),
                    OrderEnd = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderTotalDiscount = table.Column<double>(type: "float", nullable: true)
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
                name: "orderDetails",
                columns: table => new
                {
                    IdOrderDetail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    Review = table.Column<int>(type: "int", nullable: true),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderDetails");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}

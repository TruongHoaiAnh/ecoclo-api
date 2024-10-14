using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetailsV01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_OrderIdOrder",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_productItems_ProductItemIdProItem",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_OrderIdOrder",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_ProductItemIdProItem",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "OrderIdOrder",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "ProductItemIdProItem",
                table: "orderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderIdOrder",
                table: "orderDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductItemIdProItem",
                table: "orderDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_OrderIdOrder",
                table: "orderDetails",
                column: "OrderIdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_ProductItemIdProItem",
                table: "orderDetails",
                column: "ProductItemIdProItem");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_OrderIdOrder",
                table: "orderDetails",
                column: "OrderIdOrder",
                principalTable: "orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_productItems_ProductItemIdProItem",
                table: "orderDetails",
                column: "ProductItemIdProItem",
                principalTable: "productItems",
                principalColumn: "IdProItem",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

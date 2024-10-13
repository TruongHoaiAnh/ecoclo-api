using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscountV02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "discounts");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "discounts",
                newName: "ExpiryDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "discounts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "discounts");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "discounts",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "discounts",
                type: "datetime2",
                nullable: true);
        }
    }
}

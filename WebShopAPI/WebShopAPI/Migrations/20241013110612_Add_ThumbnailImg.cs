using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_ThumbnailImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailImg",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailImg",
                table: "products");
        }
    }
}

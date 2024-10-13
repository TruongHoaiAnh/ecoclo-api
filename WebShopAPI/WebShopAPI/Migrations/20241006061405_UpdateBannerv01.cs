using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBannerv01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "banners");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "banners",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }
    }
}

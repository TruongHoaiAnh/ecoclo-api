using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatereviewV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsLike",
                table: "reviews",
                newName: "Like");

            migrationBuilder.AddColumn<int>(
                name: "Dislike",
                table: "reviews",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislike",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "Like",
                table: "reviews",
                newName: "IsLike");
        }
    }
}

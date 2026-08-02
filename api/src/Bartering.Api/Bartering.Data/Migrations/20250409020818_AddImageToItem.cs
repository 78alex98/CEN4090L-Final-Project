using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bartering.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "items",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_type",
                table: "items",
                type: "text",
                nullable: true,
                comment: "MIME Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "items");

            migrationBuilder.DropColumn(
                name: "image_type",
                table: "items");
        }
    }
}

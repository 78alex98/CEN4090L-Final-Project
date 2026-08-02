using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bartering.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleToListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "listings",
                type: "text",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "listings");
        }
    }
}

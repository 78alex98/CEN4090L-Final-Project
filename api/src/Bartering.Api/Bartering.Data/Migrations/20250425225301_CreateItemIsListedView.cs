using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bartering.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateItemIsListedView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE OR REPLACE VIEW v_item_is_listed AS " +
                                 "SELECT i.*, CASE WHEN l.id is null THEN false ELSE true END is_listed " +
                                 "FROM items i " +
                                 "LEFT JOIN listings l ON i.id = l.item_id;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW v_item_is_listed;");
        }
    }
}

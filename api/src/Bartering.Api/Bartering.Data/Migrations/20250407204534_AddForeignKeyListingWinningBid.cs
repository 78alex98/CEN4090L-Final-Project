using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bartering.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyListingWinningBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_selected",
                table: "bids");

            migrationBuilder.AlterColumn<DateTime>(
                name: "posted_date",
                table: "listings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "is_open",
                table: "listings",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "closed_date",
                table: "listings",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "winning_bid_id",
                table: "listings",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "posted_date",
                table: "bids",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "ix_listings_winning_bid_id",
                table: "listings",
                column: "winning_bid_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_listings_bids_winning_bid_id",
                table: "listings",
                column: "winning_bid_id",
                principalTable: "bids",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_listings_bids_winning_bid_id",
                table: "listings");

            migrationBuilder.DropIndex(
                name: "ix_listings_winning_bid_id",
                table: "listings");

            migrationBuilder.DropColumn(
                name: "winning_bid_id",
                table: "listings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "posted_date",
                table: "listings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<bool>(
                name: "is_open",
                table: "listings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "closed_date",
                table: "listings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "posted_date",
                table: "bids",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<bool>(
                name: "is_selected",
                table: "bids",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

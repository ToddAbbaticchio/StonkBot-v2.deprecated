using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    /// <inheritdoc />
    public partial class SplitFourHandFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FourHand",
                table: "IAHistoricalData",
                newName: "LastFHTarget");

            migrationBuilder.AddColumn<string>(
                name: "FHTargetDay",
                table: "IAHistoricalData",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FHTargetDay",
                table: "IAHistoricalData");

            migrationBuilder.RenameColumn(
                name: "LastFHTarget",
                table: "IAHistoricalData",
                newName: "FourHand");
        }
    }
}

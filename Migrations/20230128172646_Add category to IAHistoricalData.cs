using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    /// <inheritdoc />
    public partial class AddcategorytoIAHistoricalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "IAHistoricalData",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "IAHistoricalData");
        }
    }
}

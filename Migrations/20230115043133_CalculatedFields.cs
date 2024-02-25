using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    /// <inheritdoc />
    public partial class CalculatedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VolumeAlert",
                table: "IAHistoricalData",
                newName: "CalculatedFields_VolumeAlert");

            migrationBuilder.RenameColumn(
                name: "FromYesterday",
                table: "IAHistoricalData",
                newName: "CalculatedFields_FromYesterday");

            migrationBuilder.RenameColumn(
                name: "FiveDayStable",
                table: "IAHistoricalData",
                newName: "CalculatedFields_FiveDayStable");

            migrationBuilder.AddColumn<string>(
                name: "CalculatedFields_AboveUpperShadow",
                table: "IAHistoricalData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalculatedFields_FourHand",
                table: "IAHistoricalData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CalculatedFields_UpToday",
                table: "IAHistoricalData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CalculatedFields_UpperShadow",
                table: "IAHistoricalData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalculatedFields_VolumeAlert2",
                table: "IAHistoricalData",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculatedFields_AboveUpperShadow",
                table: "IAHistoricalData");

            migrationBuilder.DropColumn(
                name: "CalculatedFields_FourHand",
                table: "IAHistoricalData");

            migrationBuilder.DropColumn(
                name: "CalculatedFields_UpToday",
                table: "IAHistoricalData");

            migrationBuilder.DropColumn(
                name: "CalculatedFields_UpperShadow",
                table: "IAHistoricalData");

            migrationBuilder.DropColumn(
                name: "CalculatedFields_VolumeAlert2",
                table: "IAHistoricalData");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_VolumeAlert",
                table: "IAHistoricalData",
                newName: "VolumeAlert");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_FromYesterday",
                table: "IAHistoricalData",
                newName: "FromYesterday");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_FiveDayStable",
                table: "IAHistoricalData",
                newName: "FiveDayStable");
        }
    }
}

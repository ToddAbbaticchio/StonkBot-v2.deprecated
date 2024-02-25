using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    /// <inheritdoc />
    public partial class MergeBackCalcFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CalculatedFields_VolumeAlert2",
                table: "IAHistoricalData",
                newName: "VolumeAlert2");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_VolumeAlert",
                table: "IAHistoricalData",
                newName: "VolumeAlert");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_UpperShadow",
                table: "IAHistoricalData",
                newName: "UpperShadow");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_UpToday",
                table: "IAHistoricalData",
                newName: "UpToday");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_FromYesterday",
                table: "IAHistoricalData",
                newName: "FromYesterday");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_FourHand",
                table: "IAHistoricalData",
                newName: "FourHand");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_FiveDayStable",
                table: "IAHistoricalData",
                newName: "FiveDayStable");

            migrationBuilder.RenameColumn(
                name: "CalculatedFields_AboveUpperShadow",
                table: "IAHistoricalData",
                newName: "AboveUpperShadow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VolumeAlert2",
                table: "IAHistoricalData",
                newName: "CalculatedFields_VolumeAlert2");

            migrationBuilder.RenameColumn(
                name: "VolumeAlert",
                table: "IAHistoricalData",
                newName: "CalculatedFields_VolumeAlert");

            migrationBuilder.RenameColumn(
                name: "UpperShadow",
                table: "IAHistoricalData",
                newName: "CalculatedFields_UpperShadow");

            migrationBuilder.RenameColumn(
                name: "UpToday",
                table: "IAHistoricalData",
                newName: "CalculatedFields_UpToday");

            migrationBuilder.RenameColumn(
                name: "FromYesterday",
                table: "IAHistoricalData",
                newName: "CalculatedFields_FromYesterday");

            migrationBuilder.RenameColumn(
                name: "FourHand",
                table: "IAHistoricalData",
                newName: "CalculatedFields_FourHand");

            migrationBuilder.RenameColumn(
                name: "FiveDayStable",
                table: "IAHistoricalData",
                newName: "CalculatedFields_FiveDayStable");

            migrationBuilder.RenameColumn(
                name: "AboveUpperShadow",
                table: "IAHistoricalData",
                newName: "CalculatedFields_AboveUpperShadow");
        }
    }
}

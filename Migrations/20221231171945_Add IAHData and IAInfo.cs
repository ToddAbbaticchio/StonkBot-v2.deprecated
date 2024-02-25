using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class AddIAHDataandIAInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IAInfo",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    GICSSector = table.Column<string>(type: "TEXT", nullable: false),
                    Industry = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    MarketCap = table.Column<decimal>(type: "TEXT", nullable: false),
                    AvgVolume = table.Column<float>(type: "REAL", nullable: false),
                    InSpy = table.Column<bool>(type: "INTEGER", nullable: false),
                    InQQQ = table.Column<bool>(type: "INTEGER", nullable: false),
                    InIWM = table.Column<bool>(type: "INTEGER", nullable: false),
                    InDIA = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAInfo", x => x.Symbol);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IAInfo");
        }
    }
}

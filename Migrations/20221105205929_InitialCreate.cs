using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthTokens",
                columns: table => new
                {
                    access_token = table.Column<string>(type: "TEXT", nullable: false),
                    refresh_token = table.Column<string>(type: "TEXT", nullable: false),
                    token_type = table.Column<string>(type: "TEXT", nullable: false),
                    expires_in = table.Column<int>(type: "INTEGER", nullable: false),
                    scope = table.Column<string>(type: "TEXT", nullable: false),
                    refresh_token_expires_in = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "IpoListings",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    OfferingPrice = table.Column<string>(type: "TEXT", nullable: false),
                    OfferAmmount = table.Column<string>(type: "TEXT", nullable: true),
                    OfferingEndDate = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedListingDate = table.Column<string>(type: "TEXT", nullable: false),
                    ScrapeDate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthTokens");

            migrationBuilder.DropTable(
                name: "IpoListings");
        }
    }
}

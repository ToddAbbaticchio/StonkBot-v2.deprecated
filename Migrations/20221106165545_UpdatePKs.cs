using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class UpdatePKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DbKey",
                table: "IpoListings",
                newName: "PrimaryKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthTokens",
                table: "AuthTokens",
                column: "PrimaryKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthTokens",
                table: "AuthTokens");

            migrationBuilder.RenameColumn(
                name: "PrimaryKey",
                table: "IpoListings",
                newName: "DbKey");
        }
    }
}

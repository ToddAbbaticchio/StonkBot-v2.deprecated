using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class AddedKeysToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DbKey",
                table: "IpoListings",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DbKey",
                table: "AuthTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_IpoListings",
                table: "IpoListings",
                column: "DbKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IpoListings",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "DbKey",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "DbKey",
                table: "AuthTokens");
        }
    }
}

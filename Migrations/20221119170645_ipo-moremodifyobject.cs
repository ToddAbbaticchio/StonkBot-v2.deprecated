using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class ipomoremodifyobject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurpassedOpeningDay",
                table: "IpoListings");

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscordMessageDate",
                table: "IpoListings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<ulong>(
                name: "DiscordMessageId",
                table: "IpoListings",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordMessageDate",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "DiscordMessageId",
                table: "IpoListings");

            migrationBuilder.AddColumn<bool>(
                name: "SurpassedOpeningDay",
                table: "IpoListings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}

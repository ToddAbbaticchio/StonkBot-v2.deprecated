using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class AuthKeyToUnixTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DbKey",
                table: "AuthTokens");

            migrationBuilder.AddColumn<long>(
                name: "tokenCreatedTime",
                table: "AuthTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tokenCreatedTime",
                table: "AuthTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "DbKey",
                table: "AuthTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StonkBot.Migrations
{
    public partial class UpdateIpoListing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IpoListings",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "PrimaryKey",
                table: "IpoListings");

            migrationBuilder.AddColumn<decimal>(
                name: "Close",
                table: "IpoListings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "High",
                table: "IpoListings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Low",
                table: "IpoListings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Open",
                table: "IpoListings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "IpoListings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IpoListings",
                table: "IpoListings",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IpoListings",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "Close",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "High",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "Low",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "IpoListings");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "IpoListings");

            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryKey",
                table: "IpoListings",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_IpoListings",
                table: "IpoListings",
                column: "PrimaryKey");
        }
    }
}

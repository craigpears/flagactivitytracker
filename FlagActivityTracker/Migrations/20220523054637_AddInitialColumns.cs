using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddInitialColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Pirates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CrewLink",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FlagLink",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PirateLink",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PirateName",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Flags",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Crews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CrewName",
                table: "Crews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "CrewLink",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "FlagLink",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "PirateLink",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "PirateName",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Crews");

            migrationBuilder.DropColumn(
                name: "CrewName",
                table: "Crews");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddFlagProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErrorCount",
                table: "Flags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastErrorDate",
                table: "Flags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastParsedDate",
                table: "Flags",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorCount",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "LastErrorDate",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "LastParsedDate",
                table: "Flags");
        }
    }
}

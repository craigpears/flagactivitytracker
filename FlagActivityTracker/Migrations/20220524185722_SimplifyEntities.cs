using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class SimplifyEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorCount",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "LastErrorDate",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "ErrorCount",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "LastErrorDate",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "ErrorCount",
                table: "Crews");

            migrationBuilder.DropColumn(
                name: "LastErrorDate",
                table: "Crews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErrorCount",
                table: "Pirates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastErrorDate",
                table: "Pirates",
                type: "datetime2",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "ErrorCount",
                table: "Crews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastErrorDate",
                table: "Crews",
                type: "datetime2",
                nullable: true);
        }
    }
}

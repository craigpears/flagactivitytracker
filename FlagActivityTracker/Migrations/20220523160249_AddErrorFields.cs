using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddErrorFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorCount",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "LastErrorDate",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "ErrorCount",
                table: "Crews");

            migrationBuilder.DropColumn(
                name: "LastErrorDate",
                table: "Crews");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddLastParsedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastParsedDate",
                table: "Crews",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastParsedDate",
                table: "Crews");
        }
    }
}

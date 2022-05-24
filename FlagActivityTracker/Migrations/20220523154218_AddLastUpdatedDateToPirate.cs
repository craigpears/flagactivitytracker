using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddLastUpdatedDateToPirate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndependentPirate",
                table: "Pirates");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "Pirates",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "Pirates");

            migrationBuilder.AddColumn<bool>(
                name: "IndependentPirate",
                table: "Pirates",
                type: "bit",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddVoyages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoyageId",
                table: "JobbingActivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Voyages",
                columns: table => new
                {
                    VoyageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CrewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voyages", x => x.VoyageId);
                    table.ForeignKey(
                        name: "FK_Voyages_Crews_CrewId",
                        column: x => x.CrewId,
                        principalTable: "Crews",
                        principalColumn: "CrewId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobbingActivities_VoyageId",
                table: "JobbingActivities",
                column: "VoyageId");

            migrationBuilder.CreateIndex(
                name: "IX_Voyages_CrewId",
                table: "Voyages",
                column: "CrewId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobbingActivities_Voyages_VoyageId",
                table: "JobbingActivities",
                column: "VoyageId",
                principalTable: "Voyages",
                principalColumn: "VoyageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobbingActivities_Voyages_VoyageId",
                table: "JobbingActivities");

            migrationBuilder.DropTable(
                name: "Voyages");

            migrationBuilder.DropIndex(
                name: "IX_JobbingActivities_VoyageId",
                table: "JobbingActivities");

            migrationBuilder.DropColumn(
                name: "VoyageId",
                table: "JobbingActivities");
        }
    }
}

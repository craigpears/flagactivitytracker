using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddJobbingActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobbingActivities",
                columns: table => new
                {
                    JobbingActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PirateId = table.Column<int>(type: "int", nullable: false),
                    CrewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobbingActivities", x => x.JobbingActivityId);
                    table.ForeignKey(
                        name: "FK_JobbingActivities_Crews_CrewId",
                        column: x => x.CrewId,
                        principalTable: "Crews",
                        principalColumn: "CrewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobbingActivities_Pirates_PirateId",
                        column: x => x.PirateId,
                        principalTable: "Pirates",
                        principalColumn: "PirateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobbingActivities_CrewId",
                table: "JobbingActivities",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_JobbingActivities_PirateId",
                table: "JobbingActivities",
                column: "PirateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobbingActivities");
        }
    }
}

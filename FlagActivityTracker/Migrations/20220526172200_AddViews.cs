using System;
using FlagActivityTracker.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    [DbContext(typeof(FlagActivityTrackerDbContext))]
    [Migration("20220526172200_AddViews")]
    public partial class AddViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW vVoyages
                AS 

	            SELECT v.VoyageID, v.CrewId, v.StartTime, v.EndTime, c.CrewName, COUNT(DISTINCT ja.PirateId) PirateCount
	            FROM Voyages v 
	            JOIN Crews c ON c.CrewId = v.CrewId
	            JOIN JobbingActivities ja ON ja.VoyageId = v.VoyageId
	            GROUP BY v.VoyageID, v.CrewId, v.StartTime, v.EndTime, c.CrewName
            ");

            migrationBuilder.Sql(@"CREATE VIEW vLargeVoyages
                AS 

	            SELECT *
                FROM vVoyages
                WHERE PirateCount > 10
            ");

            migrationBuilder.Sql(@"CREATE VIEW vPirateLargeVoyageCounts
                AS 

	            SELECT PirateName, COUNT(DISTINCT ja.VoyageId) Voyages
                FROM vLargeVoyages v
                JOIN JobbingActivities ja ON ja.VoyageId = v.VoyageId
                JOIN Pirates p ON p.PirateId = ja.PirateId
                GROUP BY p.PirateName
            ");

            migrationBuilder.Sql(@"CREATE VIEW vCrews
                AS 

	            SELECT c.CrewId, c.CrewName, f.FlagId, f.FlagName, COUNT(*) ActivePiratesCount
                FROM Crews c
                LEFT JOIN Flags f ON c.FlagId = f.FlagId
                JOIN Pirates p ON c.CrewId = p.CrewId
                GROUP BY c.CrewId, c.CrewName, f.FlagId, f.FlagName
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW vPiratesWithoutNonPiracyExperience
                AS

                SELECT *
                FROM Pirates p
                WHERE NOT EXISTS(
	                SELECT TOP 1 1
	                FROM Skill s
	                WHERE SkillType IN (1,2)
	                AND s.PirateId = p.PirateId
	                AND s.Experience > 3
                )
                AND PirateId IN (SELECT PirateId FROM Skill)
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW vVoyages");
            migrationBuilder.Sql("DROP VIEW vLargeVoyages");
            migrationBuilder.Sql("DROP VIEW vPirateLargeVoyageCounts");
            migrationBuilder.Sql("DROP VIEW vCrews");
            migrationBuilder.Sql("DROP VIEW vCrews");
            migrationBuilder.Sql("DROP VIEW vPiratesWithoutNonPiracyExperience");

        }
    }
}

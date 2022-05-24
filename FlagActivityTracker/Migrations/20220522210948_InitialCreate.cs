using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flags",
                columns: table => new
                {
                    FlagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flags", x => x.FlagId);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    CrewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.CrewId);
                    table.ForeignKey(
                        name: "FK_Crews_Flags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "Flags",
                        principalColumn: "FlagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pirates",
                columns: table => new
                {
                    PirateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pirates", x => x.PirateId);
                    table.ForeignKey(
                        name: "FK_Pirates_Crews_CrewId",
                        column: x => x.CrewId,
                        principalTable: "Crews",
                        principalColumn: "CrewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Crews_FlagId",
                table: "Crews",
                column: "FlagId");

            migrationBuilder.CreateIndex(
                name: "IX_Pirates_CrewId",
                table: "Pirates",
                column: "CrewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pirates");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Flags");
        }
    }
}

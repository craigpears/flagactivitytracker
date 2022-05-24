using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddVoyageCrew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voyages_Crews_CrewId",
                table: "Voyages");

            migrationBuilder.AlterColumn<int>(
                name: "CrewId",
                table: "Voyages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Voyages_Crews_CrewId",
                table: "Voyages",
                column: "CrewId",
                principalTable: "Crews",
                principalColumn: "CrewId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voyages_Crews_CrewId",
                table: "Voyages");

            migrationBuilder.AlterColumn<int>(
                name: "CrewId",
                table: "Voyages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Voyages_Crews_CrewId",
                table: "Voyages",
                column: "CrewId",
                principalTable: "Crews",
                principalColumn: "CrewId");
        }
    }
}

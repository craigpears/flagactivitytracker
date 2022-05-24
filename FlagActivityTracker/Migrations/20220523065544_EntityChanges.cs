using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class EntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crews_Flags_FlagId",
                table: "Crews");

            migrationBuilder.DropForeignKey(
                name: "FK_Pirates_Crews_CrewId",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "CrewLink",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "FlagLink",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "PirateLink",
                table: "Pirates");

            migrationBuilder.AlterColumn<int>(
                name: "CrewId",
                table: "Pirates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PPFlagId",
                table: "Flags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FlagId",
                table: "Crews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CrewName",
                table: "Crews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "PPCrewId",
                table: "Crews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Crews_Flags_FlagId",
                table: "Crews",
                column: "FlagId",
                principalTable: "Flags",
                principalColumn: "FlagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pirates_Crews_CrewId",
                table: "Pirates",
                column: "CrewId",
                principalTable: "Crews",
                principalColumn: "CrewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crews_Flags_FlagId",
                table: "Crews");

            migrationBuilder.DropForeignKey(
                name: "FK_Pirates_Crews_CrewId",
                table: "Pirates");

            migrationBuilder.DropColumn(
                name: "PPFlagId",
                table: "Flags");

            migrationBuilder.DropColumn(
                name: "PPCrewId",
                table: "Crews");

            migrationBuilder.AlterColumn<int>(
                name: "CrewId",
                table: "Pirates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrewLink",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FlagLink",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PirateLink",
                table: "Pirates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "FlagId",
                table: "Crews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CrewName",
                table: "Crews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Crews_Flags_FlagId",
                table: "Crews",
                column: "FlagId",
                principalTable: "Flags",
                principalColumn: "FlagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pirates_Crews_CrewId",
                table: "Pirates",
                column: "CrewId",
                principalTable: "Crews",
                principalColumn: "CrewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

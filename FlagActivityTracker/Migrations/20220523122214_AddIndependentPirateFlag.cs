using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    public partial class AddIndependentPirateFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IndependentPirate",
                table: "Pirates",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndependentPirate",
                table: "Pirates");
        }
    }
}

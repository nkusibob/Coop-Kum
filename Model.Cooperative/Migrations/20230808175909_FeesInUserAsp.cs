using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class FeesInUserAsp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Town",
                table: "Membre");

            migrationBuilder.AddColumn<int>(
                name: "Fees",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "Membre",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

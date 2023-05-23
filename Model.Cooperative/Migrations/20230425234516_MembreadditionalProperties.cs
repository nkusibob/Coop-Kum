using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class MembreadditionalProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Membre",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Membre",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrandparentTag",
                table: "Membre",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "Membre",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Membre");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Membre");

            migrationBuilder.DropColumn(
                name: "GrandparentTag",
                table: "Membre");

            migrationBuilder.DropColumn(
                name: "Town",
                table: "Membre");
        }
    }
}

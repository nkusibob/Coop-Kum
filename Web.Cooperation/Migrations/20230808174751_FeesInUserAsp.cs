using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Cooperation.Migrations
{
    public partial class FeesInUserAsp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fees",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "AspNetUsers");
        }
    }
}

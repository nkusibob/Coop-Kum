using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class startingDateForStepProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "StepProject");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "StepProject",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "StepProject");

            migrationBuilder.AddColumn<string>(
                name: "ReviewDate",
                table: "StepProject",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

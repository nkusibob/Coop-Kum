using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class employeePerstep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId");
        }
    }
}

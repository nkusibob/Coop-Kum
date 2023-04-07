using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class creatingEmploye : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
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

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "StepProject",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId1",
                table: "StepProject",
                column: "EmployeeId1",
                unique: true,
                filter: "[EmployeeId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_Employee_EmployeeId1",
                table: "StepProject",
                column: "EmployeeId1",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

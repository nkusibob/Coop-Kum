using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class listofstepsForemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "StepProject",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Employee",
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

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ProjectId",
                table: "Employee",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Project_ProjectId",
                table: "Employee",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_Employee_EmployeeId1",
                table: "StepProject",
                column: "EmployeeId1",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Project_ProjectId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ProjectId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId",
                unique: true);
        }
    }
}

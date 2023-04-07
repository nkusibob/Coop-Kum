using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class EmployeeLinkedSeveralStepProjectsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject");

            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId1",
                table: "StepProject");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "StepProject");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "StepProject",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StepProjectId",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_StepProjectId",
                table: "Employee",
                column: "StepProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_StepProject_StepProjectId",
                table: "Employee",
                column: "StepProjectId",
                principalTable: "StepProject",
                principalColumn: "StepProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_StepProject_StepProjectId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_Employee_StepProjectId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "StepProjectId",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "StepProject",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "StepProject",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId1",
                table: "StepProject",
                column: "EmployeeId1",
                unique: true,
                filter: "[EmployeeId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

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

using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class CreatingemployeealsoGeneartStpprojectForthisemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "StepProject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "StepProjectId",
                table: "Employee",
                type: "int",
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
    }
}

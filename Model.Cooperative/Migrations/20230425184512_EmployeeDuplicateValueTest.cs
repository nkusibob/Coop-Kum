using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class EmployeeDuplicateValueTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Project_ProjectId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_StepProject_StepProjectId1",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_Employee_EmployeeId",
                table: "StepProject");

          

            migrationBuilder.DropIndex(
                name: "IX_Employee_ProjectId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_StepProjectId1",
                table: "Employee");

          

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "StepProjectId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "StepProjectId1",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "StepProject",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

          

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpenseBudget",
                table: "Manager",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

          

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

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpenseBudget",
                table: "Manager",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepProjectId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

           

            
            migrationBuilder.CreateIndex(
                name: "IX_Employee_ProjectId",
                table: "Employee",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_StepProjectId1",
                table: "Employee",
                column: "StepProjectId1");

           

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Project_ProjectId",
                table: "Employee",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
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

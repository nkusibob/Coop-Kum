using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class ComposedPrimarykeyEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.AddColumn<int>(
                name: "StepProjectId",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StepProjectId1",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_StepProjectId1",
                table: "Employee",
                column: "StepProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_StepProject_StepProjectId1",
                table: "Employee",
                column: "StepProjectId1",
                principalTable: "StepProject",
                principalColumn: "StepProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_StepProject_StepProjectId1",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

            migrationBuilder.DropIndex(
                name: "IX_Employee_StepProjectId1",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "StepProjectId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "StepProjectId1",
                table: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId",
                unique: true);
        }
    }
}

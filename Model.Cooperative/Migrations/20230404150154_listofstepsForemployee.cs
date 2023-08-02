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
                name: "ProjectId",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject",
                column: "EmployeeId");

           
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

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Project_ProjectId",
                table: "Employee");

            

            migrationBuilder.DropIndex(
                name: "IX_StepProject_EmployeeId",
                table: "StepProject");

           

            migrationBuilder.DropIndex(
                name: "IX_Employee_ProjectId",
                table: "Employee");

            

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

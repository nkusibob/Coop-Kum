using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class addingexistingEmployeeSteProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Coop_CoopIdCoop",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_CoopIdCoop",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CoopIdCoop",
                table: "Person");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectedPersonId",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeId1",
                table: "Employee",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_EmployeeId1",
                table: "Employee",
                column: "EmployeeId1",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_EmployeeId1",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmployeeId1",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SelectedPersonId",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "CoopIdCoop",
                table: "Person",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_CoopIdCoop",
                table: "Person",
                column: "CoopIdCoop");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Coop_CoopIdCoop",
                table: "Person",
                column: "CoopIdCoop",
                principalTable: "Coop",
                principalColumn: "IdCoop",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

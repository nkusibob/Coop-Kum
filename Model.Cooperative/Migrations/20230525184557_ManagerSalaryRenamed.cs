using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class ManagerSalaryRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
              name: "Salary",
              table: "Manager",
              newName: "ManagerSalary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "ManagerSalary",
               table: "Manager",
               newName: "Salary");

        }
    }
}

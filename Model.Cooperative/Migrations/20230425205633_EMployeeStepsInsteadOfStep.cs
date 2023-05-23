using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class EMployeeStepsInsteadOfStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StepBuget",
                table: "StepProject",
                newName: "StepBudget");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StepBudget",
                table: "StepProject",
                newName: "StepBuget");
        }
    }
}

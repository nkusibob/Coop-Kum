using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class stepCatFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_StepCategorie_StepCategorieId",
                table: "StepProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StepCategorie",
                table: "StepCategorie");

            migrationBuilder.RenameTable(
                name: "StepCategorie",
                newName: "StepCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StepCategories",
                table: "StepCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_StepCategories_StepCategorieId",
                table: "StepProject",
                column: "StepCategorieId",
                principalTable: "StepCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_StepCategories_StepCategorieId",
                table: "StepProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StepCategories",
                table: "StepCategories");

            migrationBuilder.RenameTable(
                name: "StepCategories",
                newName: "StepCategorie");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StepCategorie",
                table: "StepCategorie",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_StepCategorie_StepCategorieId",
                table: "StepProject",
                column: "StepCategorieId",
                principalTable: "StepCategorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

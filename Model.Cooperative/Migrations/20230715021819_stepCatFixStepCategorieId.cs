using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class stepCatFixStepCategorieId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_StepCategories_StepCategorieId",
                table: "StepProject");

            migrationBuilder.DropColumn(
                name: "StepCategoryId",
                table: "StepProject");

            migrationBuilder.AlterColumn<int>(
                name: "StepCategorieId",
                table: "StepProject",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_StepCategories_StepCategorieId",
                table: "StepProject",
                column: "StepCategorieId",
                principalTable: "StepCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_StepCategories_StepCategorieId",
                table: "StepProject");

            migrationBuilder.AlterColumn<int>(
                name: "StepCategorieId",
                table: "StepProject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "StepCategoryId",
                table: "StepProject",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_StepCategories_StepCategorieId",
                table: "StepProject",
                column: "StepCategorieId",
                principalTable: "StepCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

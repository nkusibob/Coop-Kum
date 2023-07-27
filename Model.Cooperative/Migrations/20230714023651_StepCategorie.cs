using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class StepCategorie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StepCategorieId",
                table: "StepProject",
                nullable: true);

          

            migrationBuilder.CreateTable(
                name: "StepCategorie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepCategorie", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepProject_StepCategorieId",
                table: "StepProject",
                column: "StepCategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepProject_StepCategorie_StepCategorieId",
                table: "StepProject",
                column: "StepCategorieId",
                principalTable: "StepCategorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepProject_StepCategorie_StepCategorieId",
                table: "StepProject");

            migrationBuilder.DropTable(
                name: "StepCategorie");

            migrationBuilder.DropIndex(
                name: "IX_StepProject_StepCategorieId",
                table: "StepProject");

            migrationBuilder.DropColumn(
                name: "StepCategorieId",
                table: "StepProject");

            migrationBuilder.DropColumn(
                name: "StepCategoryId",
                table: "StepProject");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class MembreToCoop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membre_Coop_CoopIdCoop",
                table: "Membre");

            migrationBuilder.DropIndex(
                name: "IX_Membre_CoopIdCoop",
                table: "Membre");

            migrationBuilder.DropColumn(
                name: "CoopIdCoop",
                table: "Membre");

            migrationBuilder.AddColumn<int>(
                name: "MyCoopIdCoop",
                table: "Membre",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membre_MyCoopIdCoop",
                table: "Membre",
                column: "MyCoopIdCoop");

            migrationBuilder.AddForeignKey(
                name: "FK_Membre_Coop_MyCoopIdCoop",
                table: "Membre",
                column: "MyCoopIdCoop",
                principalTable: "Coop",
                principalColumn: "IdCoop",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membre_Coop_MyCoopIdCoop",
                table: "Membre");

            migrationBuilder.DropIndex(
                name: "IX_Membre_MyCoopIdCoop",
                table: "Membre");

            migrationBuilder.DropColumn(
                name: "MyCoopIdCoop",
                table: "Membre");

            migrationBuilder.AddColumn<int>(
                name: "CoopIdCoop",
                table: "Membre",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membre_CoopIdCoop",
                table: "Membre",
                column: "CoopIdCoop");

            migrationBuilder.AddForeignKey(
                name: "FK_Membre_Coop_CoopIdCoop",
                table: "Membre",
                column: "CoopIdCoop",
                principalTable: "Coop",
                principalColumn: "IdCoop",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

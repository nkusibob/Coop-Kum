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
                name: "SelectedPersonId",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

           

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
         

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

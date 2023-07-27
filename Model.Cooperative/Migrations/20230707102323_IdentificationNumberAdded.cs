using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class IdentificationNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdentificationNumber",
                table: "Livestock",
                nullable: true,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Livestock_IdentificationNumber",
            //    table: "Livestock",
            //    column: "IdentificationNumber",
            //    unique: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "IdentificationNumber",
               table: "Livestock");
        }
    }
}

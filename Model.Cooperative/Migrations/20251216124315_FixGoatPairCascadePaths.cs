using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Cooperative.Migrations
{
    /// <inheritdoc />
    public partial class FixGoatPairCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Livestock_FatherId",
                table: "Livestock");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Livestock_MotherId",
                table: "Livestock");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Livestock_FatherId",
                table: "Livestock",
                column: "FatherId",
                principalTable: "Livestock",
                principalColumn: "LivestockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Livestock_MotherId",
                table: "Livestock",
                column: "MotherId",
                principalTable: "Livestock",
                principalColumn: "LivestockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Livestock_FatherId",
                table: "Livestock");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Livestock_MotherId",
                table: "Livestock");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Livestock_FatherId",
                table: "Livestock",
                column: "FatherId",
                principalTable: "Livestock",
                principalColumn: "LivestockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Livestock_MotherId",
                table: "Livestock",
                column: "MotherId",
                principalTable: "Livestock",
                principalColumn: "LivestockId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

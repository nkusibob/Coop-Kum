using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class RenameCooperativeIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Coop_CooperativeIdCoop",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_CooperativeIdCoop",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "CooperativeIdCoop",
                table: "Livestock");

            migrationBuilder.AddColumn<int>(
                name: "CoopId",
                table: "Livestock",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GoatPairs",
                columns: table => new
                {
                    GoatPairId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstGoatLivestockId = table.Column<int>(nullable: true),
                    SecondGoatLivestockId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoatPairs", x => x.GoatPairId);
                    table.ForeignKey(
                        name: "FK_GoatPairs_Livestock_FirstGoatLivestockId",
                        column: x => x.FirstGoatLivestockId,
                        principalTable: "Livestock",
                        principalColumn: "LivestockId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GoatPairs_Livestock_SecondGoatLivestockId",
                        column: x => x.SecondGoatLivestockId,
                        principalTable: "Livestock",
                        principalColumn: "LivestockId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_CoopId",
                table: "Livestock",
                column: "CoopId");

            migrationBuilder.CreateIndex(
                name: "IX_GoatPairs_FirstGoatLivestockId",
                table: "GoatPairs",
                column: "FirstGoatLivestockId");

            migrationBuilder.CreateIndex(
                name: "IX_GoatPairs_SecondGoatLivestockId",
                table: "GoatPairs",
                column: "SecondGoatLivestockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Coop_CoopId",
                table: "Livestock",
                column: "CoopId",
                principalTable: "Coop",
                principalColumn: "IdCoop",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Coop_CoopId",
                table: "Livestock");

            migrationBuilder.DropTable(
                name: "GoatPairs");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_CoopId",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "CoopId",
                table: "Livestock");

            migrationBuilder.AddColumn<int>(
                name: "CooperativeIdCoop",
                table: "Livestock",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_CooperativeIdCoop",
                table: "Livestock",
                column: "CooperativeIdCoop");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Coop_CooperativeIdCoop",
                table: "Livestock",
                column: "CooperativeIdCoop",
                principalTable: "Coop",
                principalColumn: "IdCoop",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

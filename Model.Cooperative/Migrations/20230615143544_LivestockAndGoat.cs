using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class LivestockAndGoat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livestock",
                columns: table => new
                {
                    LivestockId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Age = table.Column<double>(nullable: false),
                    IsSold = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    IsAlive = table.Column<bool>(nullable: false),
                    IsPregnant = table.Column<bool>(nullable: false),
                    MotherId = table.Column<int>(nullable: false),
                    FatherId = table.Column<int>(nullable: false),
                    LastDropped = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    NumFemalesPaired = table.Column<int>(nullable: false),
                    CooperativeIdCoop = table.Column<int>(nullable: true),
                    LivestockType = table.Column<string>(nullable: false),
                    MotherLivestockId = table.Column<int>(nullable: false),
                    GoatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livestock", x => x.LivestockId);
                    table.ForeignKey(
                        name: "FK_Livestock_Coop_CooperativeIdCoop",
                        column: x => x.CooperativeIdCoop,
                        principalTable: "Coop",
                        principalColumn: "IdCoop",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Livestock_Livestock_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Livestock",
                        principalColumn: "LivestockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Livestock_Livestock_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Livestock",
                        principalColumn: "LivestockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_CooperativeIdCoop",
                table: "Livestock",
                column: "CooperativeIdCoop");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_FatherId",
                table: "Livestock",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_MotherId",
                table: "Livestock",
                column: "MotherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livestock");
        }
    }
}

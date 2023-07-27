using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class imgByteadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockPicture");

            migrationBuilder.CreateTable(
                name: "LivestockImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(nullable: false),
                    LivestockId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_LivestockImages_Livestock_LivestockId",
                        column: x => x.LivestockId,
                        principalTable: "Livestock",
                        principalColumn: "LivestockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockImages_LivestockId",
                table: "LivestockImages",
                column: "LivestockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockImages");

            migrationBuilder.CreateTable(
                name: "LivestockPicture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoatId = table.Column<int>(type: "int", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockPicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockPicture_Livestock_GoatId",
                        column: x => x.GoatId,
                        principalTable: "Livestock",
                        principalColumn: "LivestockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockPicture_GoatId",
                table: "LivestockPicture",
                column: "GoatId");
        }
    }
}

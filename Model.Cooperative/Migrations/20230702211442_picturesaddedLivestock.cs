using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class picturesaddedLivestock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Livestock",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Livestock",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "LivestockPicture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureUrl = table.Column<string>(nullable: true),
                    GoatId = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockPicture");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Livestock");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class uploadedMemberPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonPicture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(nullable: false),
                    PersonId = table.Column<int>(nullable: false),
                    OfflineMemberMembreId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonPicture_OfflineMember_OfflineMemberMembreId",
                        column: x => x.OfflineMemberMembreId,
                        principalTable: "OfflineMember",
                        principalColumn: "MembreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonPicture_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonPicture_OfflineMemberMembreId",
                table: "PersonPicture",
                column: "OfflineMemberMembreId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPicture_PersonId",
                table: "PersonPicture",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonPicture");
        }
    }
}

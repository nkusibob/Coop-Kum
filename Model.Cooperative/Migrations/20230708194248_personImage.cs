using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class personImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonPicture_OfflineMember_OfflineMemberMembreId",
                table: "PersonPicture");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonPicture_Person_PersonId",
                table: "PersonPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonPicture",
                table: "PersonPicture");

            migrationBuilder.RenameTable(
                name: "PersonPicture",
                newName: "PersonImages");

            migrationBuilder.RenameIndex(
                name: "IX_PersonPicture_PersonId",
                table: "PersonImages",
                newName: "IX_PersonImages_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonPicture_OfflineMemberMembreId",
                table: "PersonImages",
                newName: "IX_PersonImages_OfflineMemberMembreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonImages",
                table: "PersonImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonImages_OfflineMember_OfflineMemberMembreId",
                table: "PersonImages",
                column: "OfflineMemberMembreId",
                principalTable: "OfflineMember",
                principalColumn: "MembreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonImages_Person_PersonId",
                table: "PersonImages",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonImages_OfflineMember_OfflineMemberMembreId",
                table: "PersonImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonImages_Person_PersonId",
                table: "PersonImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonImages",
                table: "PersonImages");

            migrationBuilder.RenameTable(
                name: "PersonImages",
                newName: "PersonPicture");

            migrationBuilder.RenameIndex(
                name: "IX_PersonImages_PersonId",
                table: "PersonPicture",
                newName: "IX_PersonPicture_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonImages_OfflineMemberMembreId",
                table: "PersonPicture",
                newName: "IX_PersonPicture_OfflineMemberMembreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonPicture",
                table: "PersonPicture",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonPicture_OfflineMember_OfflineMemberMembreId",
                table: "PersonPicture",
                column: "OfflineMemberMembreId",
                principalTable: "OfflineMember",
                principalColumn: "MembreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonPicture_Person_PersonId",
                table: "PersonPicture",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

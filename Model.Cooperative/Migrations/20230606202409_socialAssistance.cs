using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Model.Cooperative.Migrations
{
    public partial class socialAssistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialAssistances",
                columns: table => new
                {
                    socialAssistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(nullable: false),
                    IsValidated = table.Column<bool>(nullable: false),
                    IsRepayable = table.Column<bool>(nullable: false),
                    IsRepaid = table.Column<bool>(nullable: false),
                    DateReceived = table.Column<DateTime>(nullable: false),
                    DateValidated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialAssistances", x => x.socialAssistId);
                    table.ForeignKey(
                        name: "FK_SocialAssistances_Membre_socialAssistId",
                        column: x => x.socialAssistId,
                        principalTable: "Membre",
                        principalColumn: "MembreId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialAssistances");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class CountryCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdNumber",
                table: "ApplicationUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserImage",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Data = table.Column<byte[]>(nullable: false),
                    AspUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserImage_ApplicationUser_AspUserId",
                        column: x => x.AspUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserImage_AspUserId",
                table: "ApplicationUserImage",
                column: "AspUserId",
                unique: true,
                filter: "[AspUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserImage");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<int>(
                name: "IdNumber",
                table: "ApplicationUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Cooperation.Migrations
{
    public partial class PersonIdentityImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Table ApplicationUserImage already exists in the database.
            // We don't want EF to try to create it again.
            // So we deliberately skip CreateTable here.

            //migrationBuilder.CreateTable(
            //    name: "ApplicationUserImage",
            //    columns: table => new
            //    {
            //        ImageId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Data = table.Column<byte[]>(nullable: false),
            //        UserId = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ApplicationUserImage", x => x.ImageId);
            //        table.ForeignKey(
            //            name: "FK_ApplicationUserImage_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ApplicationUserImage_UserId",
            //    table: "ApplicationUserImage",
            //    column: "UserId",
            //    unique: true,
            //    filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // We are not dropping ApplicationUserImage because it already existed before this migration.

            //migrationBuilder.DropTable(
            //    name: "ApplicationUserImage");
        }
    }
}

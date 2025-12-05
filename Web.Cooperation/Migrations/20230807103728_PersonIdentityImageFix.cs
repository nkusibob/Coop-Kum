using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Cooperation.Migrations
{
    public partial class PersonIdentityImageFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // For this existing database, the FK name 'FK_ApplicationUserImage_AspNetUsers_UserId'
            // either does not exist or has a different name.
            // We want EF to move past this migration without trying to drop it.
            // If you are OK with the current FK setup in the DB, you can no-op this migration.

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ApplicationUserImage_AspNetUsers_UserId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ApplicationUserImage",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropIndex(
            //    name: "IX_ApplicationUserImage_UserId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropColumn(
            //    name: "ImageId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.AddColumn<string>(
            //    name: "Id",
            //    table: "ApplicationUserImage",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "AspUserId",
            //    table: "ApplicationUserImage",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ApplicationUserImage",
            //    table: "ApplicationUserImage",
            //    column: "Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ApplicationUserImage_AspUserId",
            //    table: "ApplicationUserImage",
            //    column: "AspUserId",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ApplicationUserImage_AspNetUsers_AspUserId",
            //    table: "ApplicationUserImage",
            //    column: "AspUserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op: we are not changing FK constraints in this migration for this database
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ApplicationUserImage_AspNetUsers_AspUserId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ApplicationUserImage",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropIndex(
            //    name: "IX_ApplicationUserImage_AspUserId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropColumn(
            //    name: "Id",
            //    table: "ApplicationUserImage");

            //migrationBuilder.DropColumn(
            //    name: "AspUserId",
            //    table: "ApplicationUserImage");

            //migrationBuilder.AddColumn<int>(
            //    name: "ImageId",
            //    table: "ApplicationUserImage",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId",
            //    table: "ApplicationUserImage",
            //    type: "nvarchar(450)",
            //    nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ApplicationUserImage",
            //    table: "ApplicationUserImage",
            //    column: "ImageId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ApplicationUserImage_UserId",
            //    table: "ApplicationUserImage",
            //    column: "UserId",
            //    unique: true,
            //    filter: "[UserId] IS NOT NULL");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ApplicationUserImage_AspNetUsers_UserId",
            //    table: "ApplicationUserImage",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}

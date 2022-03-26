using Microsoft.EntityFrameworkCore.Migrations;

namespace SWZRFI.Migrations
{
    public partial class InverseRelationsForModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAccountId",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserAccountId",
                table: "Applications",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_UserAccountId",
                table: "Applications",
                column: "UserAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_UserAccountId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_UserAccountId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Applications");
        }
    }
}

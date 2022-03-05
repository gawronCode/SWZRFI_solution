using Microsoft.EntityFrameworkCore.Migrations;

namespace SWZRFI.Migrations
{
    public partial class JobOfferToApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobOfferId",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobOfferId",
                table: "Applications",
                column: "JobOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobOffers_JobOfferId",
                table: "Applications",
                column: "JobOfferId",
                principalTable: "JobOffers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobOffers_JobOfferId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_JobOfferId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "JobOfferId",
                table: "Applications");
        }
    }
}

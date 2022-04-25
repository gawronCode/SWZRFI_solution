using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SWZRFI.Migrations
{
    public partial class jobOfferMeeting_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobOfferUserMeetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    JobOfferId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    MeetingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferUserMeetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOfferUserMeetings_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobOfferUserMeetings_AspNetUsers_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobOfferUserMeetings_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferUserMeetings_ApplicationId",
                table: "JobOfferUserMeetings",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferUserMeetings_JobOfferId",
                table: "JobOfferUserMeetings",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferUserMeetings_UserAccountId",
                table: "JobOfferUserMeetings",
                column: "UserAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOfferUserMeetings");
        }
    }
}

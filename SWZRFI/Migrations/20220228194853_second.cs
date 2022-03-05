using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SWZRFI.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CvId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cvs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Expirience = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    AdditionalSkills = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Languages = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    OthersWebsites = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cvs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CvId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Opened = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applications_Cvs_CvId",
                        column: x => x.CvId,
                        principalTable: "Cvs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CvId",
                table: "AspNetUsers",
                column: "CvId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CompanyId",
                table: "Applications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CvId",
                table: "Applications",
                column: "CvId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cvs_CvId",
                table: "AspNetUsers",
                column: "CvId",
                principalTable: "Cvs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cvs_CvId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CvId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CvId",
                table: "AspNetUsers");
        }
    }
}

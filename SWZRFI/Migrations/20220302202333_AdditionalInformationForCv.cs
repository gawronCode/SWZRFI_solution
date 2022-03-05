using Microsoft.EntityFrameworkCore.Migrations;

namespace SWZRFI.Migrations
{
    public partial class AdditionalInformationForCv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Cvs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Cvs");
        }
    }
}

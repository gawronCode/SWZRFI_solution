using Microsoft.EntityFrameworkCore.Migrations;

namespace SWZRFI.Migrations
{
    public partial class questionnaireIdToJobOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int?>(
                name: "CompanyId",
                table: "Questionnaires",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int?>(
                name: "QuestionnaireId",
                table: "JobOffers",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questionnaires_CompanyId",
                table: "Questionnaires",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_QuestionnaireId",
                table: "JobOffers",
                column: "QuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Questionnaires_QuestionnaireId",
                table: "JobOffers",
                column: "QuestionnaireId",
                principalTable: "Questionnaires",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questionnaires_Companies_CompanyId",
                table: "Questionnaires",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Questionnaires_QuestionnaireId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questionnaires_Companies_CompanyId",
                table: "Questionnaires");

            migrationBuilder.DropIndex(
                name: "IX_Questionnaires_CompanyId",
                table: "Questionnaires");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_QuestionnaireId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Questionnaires");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId",
                table: "JobOffers");
        }
    }
}

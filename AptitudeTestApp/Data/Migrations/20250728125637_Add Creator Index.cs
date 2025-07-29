using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptitudeTestApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Universities_CreatorId",
                table: "Universities",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSessions_CreatorId",
                table: "TestSessions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSessionQuestions_CreatorId",
                table: "TestSessionQuestions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubmissions_CreatorId",
                table: "StudentSubmissions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_CreatorId",
                table: "StudentAnswers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_CreatorId",
                table: "QuestionOptions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCategories_CreatorId",
                table: "QuestionCategories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AntiCheatLogs_CreatorId",
                table: "AntiCheatLogs",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Universities_CreatorId",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_TestSessions_CreatorId",
                table: "TestSessions");

            migrationBuilder.DropIndex(
                name: "IX_TestSessionQuestions_CreatorId",
                table: "TestSessionQuestions");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubmissions_CreatorId",
                table: "StudentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswers_CreatorId",
                table: "StudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_CreatorId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionCategories_CreatorId",
                table: "QuestionCategories");

            migrationBuilder.DropIndex(
                name: "IX_AntiCheatLogs_CreatorId",
                table: "AntiCheatLogs");
        }
    }
}

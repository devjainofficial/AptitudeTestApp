using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptitudeTestApp.Migrations
{
    /// <inheritdoc />
    public partial class CascadeStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubmissions_TestSessions_TestSessionId",
                table: "StudentSubmissions");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubmissions_TestSessions_TestSessionId",
                table: "StudentSubmissions",
                column: "TestSessionId",
                principalTable: "TestSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubmissions_TestSessions_TestSessionId",
                table: "StudentSubmissions");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubmissions_TestSessions_TestSessionId",
                table: "StudentSubmissions",
                column: "TestSessionId",
                principalTable: "TestSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

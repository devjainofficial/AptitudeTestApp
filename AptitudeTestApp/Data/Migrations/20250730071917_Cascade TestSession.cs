using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptitudeTestApp.Migrations
{
    /// <inheritdoc />
    public partial class CascadeTestSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSessions_Universities_UniversityId",
                table: "TestSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSessions_Universities_UniversityId",
                table: "TestSessions",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSessions_Universities_UniversityId",
                table: "TestSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSessions_Universities_UniversityId",
                table: "TestSessions",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

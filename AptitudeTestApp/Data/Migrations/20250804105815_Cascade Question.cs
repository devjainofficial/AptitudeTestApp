using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptitudeTestApp.Migrations
{
    /// <inheritdoc />
    public partial class CascadeQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionCategories_CategoryId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionCategories_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "QuestionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionCategories_CategoryId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionCategories_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "QuestionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

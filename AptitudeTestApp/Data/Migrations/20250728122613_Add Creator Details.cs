using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptitudeTestApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Universities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "TestSessions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TestSessionQuestions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "TestSessionQuestions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "StudentSubmissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentAnswers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "StudentAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "QuestionOptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "QuestionOptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "QuestionCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AntiCheatLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AntiCheatLogs",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "TestSessions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TestSessionQuestions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "TestSessionQuestions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StudentSubmissions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "QuestionCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AntiCheatLogs");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AntiCheatLogs");
        }
    }
}

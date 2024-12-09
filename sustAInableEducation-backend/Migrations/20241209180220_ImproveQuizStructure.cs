using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class ImproveQuizStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMultipleChoice",
                table: "QuizQuestion",
                newName: "IsMultipleResponse");

            migrationBuilder.AddColumn<Guid>(
                name: "EnvironmentId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "IsMultipleResponse",
                table: "QuizQuestion",
                newName: "IsMultipleChoice");
        }
    }
}

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
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_EnvironmentParticipant_EnvironmentParticipantEnvironmentId_EnvironmentParticipantUserId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_EnvironmentParticipantEnvironmentId_EnvironmentParticipantUserId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "EnvironmentParticipantEnvironmentId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "EnvironmentParticipantUserId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "IsMultipleChoice",
                table: "QuizQuestion",
                newName: "IsMultipleResponse");

            migrationBuilder.AlterColumn<long>(
                name: "NumberVotes",
                table: "StoryChoice",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Length",
                table: "Story",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "EnvironmentId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "NumberQuestions",
                table: "Quiz",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
                name: "NumberQuestions",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "IsMultipleResponse",
                table: "QuizQuestion",
                newName: "IsMultipleChoice");

            migrationBuilder.AlterColumn<int>(
                name: "NumberVotes",
                table: "StoryChoice",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "Story",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<Guid>(
                name: "EnvironmentParticipantEnvironmentId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnvironmentParticipantUserId",
                table: "Quiz",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_EnvironmentParticipantEnvironmentId_EnvironmentParticipantUserId",
                table: "Quiz",
                columns: new[] { "EnvironmentParticipantEnvironmentId", "EnvironmentParticipantUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_EnvironmentParticipant_EnvironmentParticipantEnvironmentId_EnvironmentParticipantUserId",
                table: "Quiz",
                columns: new[] { "EnvironmentParticipantEnvironmentId", "EnvironmentParticipantUserId" },
                principalTable: "EnvironmentParticipant",
                principalColumns: new[] { "EnvironmentId", "UserId" });
        }
    }
}

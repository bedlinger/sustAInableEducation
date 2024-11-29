using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddVotesStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryPart_Story_StoryId",
                table: "StoryPart");

            migrationBuilder.RenameColumn(
                name: "IsTaken",
                table: "StoryChoice",
                newName: "IsVoted");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoryId",
                table: "StoryPart",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "NumberVotes",
                table: "StoryChoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "EnvironmentParticipant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "EnvironmentParticipant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryPart_Story_StoryId",
                table: "StoryPart",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryPart_Story_StoryId",
                table: "StoryPart");

            migrationBuilder.DropColumn(
                name: "NumberVotes",
                table: "StoryChoice");

            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "EnvironmentParticipant");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "EnvironmentParticipant");

            migrationBuilder.RenameColumn(
                name: "IsVoted",
                table: "StoryChoice",
                newName: "IsTaken");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoryId",
                table: "StoryPart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryPart_Story_StoryId",
                table: "StoryPart",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

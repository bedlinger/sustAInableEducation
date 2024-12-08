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
                name: "FK_Story_StoryPreset_PresetId",
                table: "Story");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryPart_Story_StoryId",
                table: "StoryPart");

            migrationBuilder.DropTable(
                name: "StoryPreset");

            migrationBuilder.DropTable(
                name: "StoryPresetPart");

            migrationBuilder.DropIndex(
                name: "IX_Story_PresetId",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "StoryChoice");

            migrationBuilder.DropColumn(
                name: "PresetId",
                table: "Story");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoryId",
                table: "StoryPart",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<long>(
                name: "ChosenNumber",
                table: "StoryPart",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VotingEndAt",
                table: "StoryPart",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberVotes",
                table: "StoryChoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "Story",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Creativity",
                table: "Story",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddColumn<long>(
                name: "VotingTimeSeconds",
                table: "Environment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
                name: "ChosenNumber",
                table: "StoryPart");

            migrationBuilder.DropColumn(
                name: "VotingEndAt",
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

            migrationBuilder.DropColumn(
                name: "VotingTimeSeconds",
                table: "Environment");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoryId",
                table: "StoryPart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "StoryChoice",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "Story",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Creativity",
                table: "Story",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "PresetId",
                table: "Story",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoryPresetPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChoiceNumber = table.Column<int>(type: "int", nullable: false),
                    ChoiceText = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    PreviousId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryPresetPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPresetPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPresetPart_StoryPresetPart_StoryPresetPartId",
                        column: x => x.StoryPresetPartId,
                        principalTable: "StoryPresetPart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoryPreset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitialPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPreGenerated = table.Column<bool>(type: "bit", nullable: false),
                    Prompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPreset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPreset_StoryPresetPart_InitialPartId",
                        column: x => x.InitialPartId,
                        principalTable: "StoryPresetPart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Story_PresetId",
                table: "Story",
                column: "PresetId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryPreset_InitialPartId",
                table: "StoryPreset",
                column: "InitialPartId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryPresetPart_StoryPresetPartId",
                table: "StoryPresetPart",
                column: "StoryPresetPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Story_StoryPreset_PresetId",
                table: "Story",
                column: "PresetId",
                principalTable: "StoryPreset",
                principalColumn: "Id");

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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class ImproveStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environment_Story_StoryId",
                table: "Environment");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_StoryPreset_PresetId",
                table: "Story");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryPreset_StoryPresetPart_InitialPartId",
                table: "StoryPreset");

            migrationBuilder.AlterColumn<string>(
                name: "ChoiceText",
                table: "StoryPresetPart",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "InitialPartId",
                table: "StoryPreset",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "IsPreGenerated",
                table: "StoryPreset",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Prompt",
                table: "StoryPreset",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "StoryChoice",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<Guid>(
                name: "PresetId",
                table: "Story",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "QuizQuestion",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "EnvironmentParticipant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "StoryId",
                table: "Environment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Environment_Story_StoryId",
                table: "Environment",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Story_StoryPreset_PresetId",
                table: "Story",
                column: "PresetId",
                principalTable: "StoryPreset",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryPreset_StoryPresetPart_InitialPartId",
                table: "StoryPreset",
                column: "InitialPartId",
                principalTable: "StoryPresetPart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environment_Story_StoryId",
                table: "Environment");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_StoryPreset_PresetId",
                table: "Story");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryPreset_StoryPresetPart_InitialPartId",
                table: "StoryPreset");

            migrationBuilder.DropColumn(
                name: "IsPreGenerated",
                table: "StoryPreset");

            migrationBuilder.DropColumn(
                name: "Prompt",
                table: "StoryPreset");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "EnvironmentParticipant");

            migrationBuilder.AlterColumn<string>(
                name: "ChoiceText",
                table: "StoryPresetPart",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<Guid>(
                name: "InitialPartId",
                table: "StoryPreset",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "StoryChoice",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "Story",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PresetId",
                table: "Story",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
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

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "QuizQuestion",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoryId",
                table: "Environment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Environment_Story_StoryId",
                table: "Environment",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Story_StoryPreset_PresetId",
                table: "Story",
                column: "PresetId",
                principalTable: "StoryPreset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryPreset_StoryPresetPart_InitialPartId",
                table: "StoryPreset",
                column: "InitialPartId",
                principalTable: "StoryPresetPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

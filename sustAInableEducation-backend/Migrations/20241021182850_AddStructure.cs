using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoryPresetPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChoiceNumber = table.Column<int>(type: "int", nullable: false),
                    ChoiceText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoryPresetPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    InitialPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPreset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPreset_StoryPresetPart_InitialPartId",
                        column: x => x.InitialPartId,
                        principalTable: "StoryPresetPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PresetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Prompt = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Creativity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Story_StoryPreset_PresetId",
                        column: x => x.PresetId,
                        principalTable: "StoryPreset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Environment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Environment_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPart_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentParticipant",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnvironmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentParticipant", x => new { x.EnvironmentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EnvironmentParticipant_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnvironmentParticipant_Environment_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryChoice",
                columns: table => new
                {
                    StoryPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    IsTaken = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryChoice", x => new { x.StoryPartId, x.Number });
                    table.ForeignKey(
                        name: "FK_StoryChoice_StoryPart_StoryPartId",
                        column: x => x.StoryPartId,
                        principalTable: "StoryPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EnvironmentParticipantEnvironmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnvironmentParticipantUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quiz_EnvironmentParticipant_EnvironmentParticipantEnvironmentId_EnvironmentParticipantUserId",
                        columns: x => new { x.EnvironmentParticipantEnvironmentId, x.EnvironmentParticipantUserId },
                        principalTable: "EnvironmentParticipant",
                        principalColumns: new[] { "EnvironmentId", "UserId" });
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    IsMultipleChoice = table.Column<bool>(type: "bit", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestion_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuizChoice",
                columns: table => new
                {
                    QuizQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizChoice", x => new { x.QuizQuestionId, x.Number });
                    table.ForeignKey(
                        name: "FK_QuizChoice_QuizQuestion_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "QuizQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizResult",
                columns: table => new
                {
                    QuizQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TryNumber = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResult", x => new { x.QuizQuestionId, x.TryNumber });
                    table.ForeignKey(
                        name: "FK_QuizResult_QuizQuestion_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "QuizQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Environment_StoryId",
                table: "Environment",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentParticipant_UserId",
                table: "EnvironmentParticipant",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_EnvironmentParticipantEnvironmentId_EnvironmentParticipantUserId",
                table: "Quiz",
                columns: new[] { "EnvironmentParticipantEnvironmentId", "EnvironmentParticipantUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestion_QuizId",
                table: "QuizQuestion",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Story_PresetId",
                table: "Story",
                column: "PresetId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryPart_StoryId",
                table: "StoryPart",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryPreset_InitialPartId",
                table: "StoryPreset",
                column: "InitialPartId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryPresetPart_StoryPresetPartId",
                table: "StoryPresetPart",
                column: "StoryPresetPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizChoice");

            migrationBuilder.DropTable(
                name: "QuizResult");

            migrationBuilder.DropTable(
                name: "StoryChoice");

            migrationBuilder.DropTable(
                name: "QuizQuestion");

            migrationBuilder.DropTable(
                name: "StoryPart");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "EnvironmentParticipant");

            migrationBuilder.DropTable(
                name: "Environment");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "StoryPreset");

            migrationBuilder.DropTable(
                name: "StoryPresetPart");
        }
    }
}

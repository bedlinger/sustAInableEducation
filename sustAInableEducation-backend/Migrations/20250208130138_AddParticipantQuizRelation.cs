using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParticipantQuizRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpaceParticipantSpaceId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpaceParticipantUserId",
                table: "Quiz",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_SpaceParticipantSpaceId_SpaceParticipantUserId",
                table: "Quiz",
                columns: new[] { "SpaceParticipantSpaceId", "SpaceParticipantUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_SpaceParticipant_SpaceParticipantSpaceId_SpaceParticipantUserId",
                table: "Quiz",
                columns: new[] { "SpaceParticipantSpaceId", "SpaceParticipantUserId" },
                principalTable: "SpaceParticipant",
                principalColumns: new[] { "SpaceId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_SpaceParticipant_SpaceParticipantSpaceId_SpaceParticipantUserId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_SpaceParticipantSpaceId_SpaceParticipantUserId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "SpaceParticipantSpaceId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "SpaceParticipantUserId",
                table: "Quiz");
        }
    }
}

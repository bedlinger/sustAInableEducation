using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddVoteImpact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "EnvironmentParticipant");

            migrationBuilder.AddColumn<float>(
                name: "VoteImpact",
                table: "EnvironmentParticipant",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteImpact",
                table: "EnvironmentParticipant");

            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "EnvironmentParticipant",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

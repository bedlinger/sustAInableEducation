using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddImpact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creativity",
                table: "Story");

            migrationBuilder.AddColumn<float>(
                name: "Impact",
                table: "StoryChoice",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Temperature",
                table: "Story",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TopP",
                table: "Story",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalImpact",
                table: "Story",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Impact",
                table: "EnvironmentParticipant",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Impact",
                table: "StoryChoice");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "TopP",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "TotalImpact",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "EnvironmentParticipant");

            migrationBuilder.AddColumn<int>(
                name: "Creativity",
                table: "Story",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

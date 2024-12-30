using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result_DiscussionQuestions",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result_Learnings",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result_NegativeChoices",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result_PositiveChoices",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result_Summary",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result_Text",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result_DiscussionQuestions",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "Result_Learnings",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "Result_NegativeChoices",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "Result_PositiveChoices",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "Result_Summary",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "Result_Text",
                table: "Story");
        }
    }
}

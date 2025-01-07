using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prompt",
                table: "Story",
                newName: "Topic");

            migrationBuilder.AddColumn<int>(
                name: "TargetGroup",
                table: "Story",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetGroup",
                table: "Story");

            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "Story",
                newName: "Prompt");
        }
    }
}

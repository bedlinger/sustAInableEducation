using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddImageGenerationDisableOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImageGenerationEnabled",
                table: "Space",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImageGenerationEnabled",
                table: "Space");
        }
    }
}

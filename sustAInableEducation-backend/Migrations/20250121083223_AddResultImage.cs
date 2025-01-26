using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sustAInableEducation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddResultImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result_Image",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result_Image",
                table: "Story");
        }
    }
}

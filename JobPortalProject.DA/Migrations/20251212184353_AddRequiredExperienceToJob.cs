using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredExperienceToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequiredExperience",
                table: "JobTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredExperience",
                table: "JobTranslations");
        }
    }
}

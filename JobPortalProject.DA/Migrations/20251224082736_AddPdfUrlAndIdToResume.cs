using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfUrlAndIdToResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumePublicId",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumePublicId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Resumes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToBio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Bios");
        }
    }
}

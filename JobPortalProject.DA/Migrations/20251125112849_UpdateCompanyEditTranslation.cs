using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyEditTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTypeTranslations_Companies_CompanyId",
                table: "CompanyTypeTranslations");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "CompanyTypeTranslations",
                newName: "CompanyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyTypeTranslations_CompanyId",
                table: "CompanyTypeTranslations",
                newName: "IX_CompanyTypeTranslations_CompanyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTypeTranslations_Companies_CompanyTypeId",
                table: "CompanyTypeTranslations",
                column: "CompanyTypeId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTypeTranslations_Companies_CompanyTypeId",
                table: "CompanyTypeTranslations");

            migrationBuilder.RenameColumn(
                name: "CompanyTypeId",
                table: "CompanyTypeTranslations",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyTypeTranslations_CompanyTypeId",
                table: "CompanyTypeTranslations",
                newName: "IX_CompanyTypeTranslations_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTypeTranslations_Companies_CompanyId",
                table: "CompanyTypeTranslations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslationsListToCompanyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTypeTranslations_Companies_CompanyTypeId",
                table: "CompanyTypeTranslations");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTypeTranslations_CompanyTypes_CompanyTypeId",
                table: "CompanyTypeTranslations",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTypeTranslations_CompanyTypes_CompanyTypeId",
                table: "CompanyTypeTranslations");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTypeTranslations_Companies_CompanyTypeId",
                table: "CompanyTypeTranslations",
                column: "CompanyTypeId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

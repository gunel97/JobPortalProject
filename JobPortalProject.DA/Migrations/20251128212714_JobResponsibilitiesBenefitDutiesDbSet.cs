using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class JobResponsibilitiesBenefitDutiesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobExtraBenefit_Jobs_JobId",
                table: "JobExtraBenefit");

            migrationBuilder.DropForeignKey(
                name: "FK_JobExtraBenefitTranslation_JobExtraBenefit_JobExtraBenefitId",
                table: "JobExtraBenefitTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_JobExtraBenefitTranslation_Languages_LanguageId",
                table: "JobExtraBenefitTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_JobMainDuty_Jobs_JobId",
                table: "JobMainDuty");

            migrationBuilder.DropForeignKey(
                name: "FK_JobMainDutyTranslation_JobMainDuty_JobMainDutyId",
                table: "JobMainDutyTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_JobMainDutyTranslation_Languages_LanguageId",
                table: "JobMainDutyTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_JobResponsibility_Jobs_JobId",
                table: "JobResponsibility");

            migrationBuilder.DropForeignKey(
                name: "FK_JobResponsibilityTranslation_JobResponsibility_JobResponsibilityId",
                table: "JobResponsibilityTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_JobResponsibilityTranslation_Languages_LanguageId",
                table: "JobResponsibilityTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobResponsibilityTranslation",
                table: "JobResponsibilityTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobResponsibility",
                table: "JobResponsibility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobMainDutyTranslation",
                table: "JobMainDutyTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobMainDuty",
                table: "JobMainDuty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobExtraBenefitTranslation",
                table: "JobExtraBenefitTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobExtraBenefit",
                table: "JobExtraBenefit");

            migrationBuilder.RenameTable(
                name: "JobResponsibilityTranslation",
                newName: "JobResponsibilityTranslations");

            migrationBuilder.RenameTable(
                name: "JobResponsibility",
                newName: "JobResponsibilities");

            migrationBuilder.RenameTable(
                name: "JobMainDutyTranslation",
                newName: "JobMainDutyTranslations");

            migrationBuilder.RenameTable(
                name: "JobMainDuty",
                newName: "JobMainDuties");

            migrationBuilder.RenameTable(
                name: "JobExtraBenefitTranslation",
                newName: "JobExtraBenefitTranslations");

            migrationBuilder.RenameTable(
                name: "JobExtraBenefit",
                newName: "JobExtraBenefits");

            migrationBuilder.RenameIndex(
                name: "IX_JobResponsibilityTranslation_LanguageId",
                table: "JobResponsibilityTranslations",
                newName: "IX_JobResponsibilityTranslations_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_JobResponsibilityTranslation_JobResponsibilityId",
                table: "JobResponsibilityTranslations",
                newName: "IX_JobResponsibilityTranslations_JobResponsibilityId");

            migrationBuilder.RenameIndex(
                name: "IX_JobResponsibility_JobId",
                table: "JobResponsibilities",
                newName: "IX_JobResponsibilities_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobMainDutyTranslation_LanguageId",
                table: "JobMainDutyTranslations",
                newName: "IX_JobMainDutyTranslations_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_JobMainDutyTranslation_JobMainDutyId",
                table: "JobMainDutyTranslations",
                newName: "IX_JobMainDutyTranslations_JobMainDutyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobMainDuty_JobId",
                table: "JobMainDuties",
                newName: "IX_JobMainDuties_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobExtraBenefitTranslation_LanguageId",
                table: "JobExtraBenefitTranslations",
                newName: "IX_JobExtraBenefitTranslations_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_JobExtraBenefitTranslation_JobExtraBenefitId",
                table: "JobExtraBenefitTranslations",
                newName: "IX_JobExtraBenefitTranslations_JobExtraBenefitId");

            migrationBuilder.RenameIndex(
                name: "IX_JobExtraBenefit_JobId",
                table: "JobExtraBenefits",
                newName: "IX_JobExtraBenefits_JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobResponsibilityTranslations",
                table: "JobResponsibilityTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobResponsibilities",
                table: "JobResponsibilities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobMainDutyTranslations",
                table: "JobMainDutyTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobMainDuties",
                table: "JobMainDuties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobExtraBenefitTranslations",
                table: "JobExtraBenefitTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobExtraBenefits",
                table: "JobExtraBenefits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobExtraBenefits_Jobs_JobId",
                table: "JobExtraBenefits",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobExtraBenefitTranslations_JobExtraBenefits_JobExtraBenefitId",
                table: "JobExtraBenefitTranslations",
                column: "JobExtraBenefitId",
                principalTable: "JobExtraBenefits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobExtraBenefitTranslations_Languages_LanguageId",
                table: "JobExtraBenefitTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobMainDuties_Jobs_JobId",
                table: "JobMainDuties",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobMainDutyTranslations_JobMainDuties_JobMainDutyId",
                table: "JobMainDutyTranslations",
                column: "JobMainDutyId",
                principalTable: "JobMainDuties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobMainDutyTranslations_Languages_LanguageId",
                table: "JobMainDutyTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobResponsibilities_Jobs_JobId",
                table: "JobResponsibilities",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobResponsibilityTranslations_JobResponsibilities_JobResponsibilityId",
                table: "JobResponsibilityTranslations",
                column: "JobResponsibilityId",
                principalTable: "JobResponsibilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobResponsibilityTranslations_Languages_LanguageId",
                table: "JobResponsibilityTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobExtraBenefits_Jobs_JobId",
                table: "JobExtraBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_JobExtraBenefitTranslations_JobExtraBenefits_JobExtraBenefitId",
                table: "JobExtraBenefitTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobExtraBenefitTranslations_Languages_LanguageId",
                table: "JobExtraBenefitTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobMainDuties_Jobs_JobId",
                table: "JobMainDuties");

            migrationBuilder.DropForeignKey(
                name: "FK_JobMainDutyTranslations_JobMainDuties_JobMainDutyId",
                table: "JobMainDutyTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobMainDutyTranslations_Languages_LanguageId",
                table: "JobMainDutyTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobResponsibilities_Jobs_JobId",
                table: "JobResponsibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_JobResponsibilityTranslations_JobResponsibilities_JobResponsibilityId",
                table: "JobResponsibilityTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobResponsibilityTranslations_Languages_LanguageId",
                table: "JobResponsibilityTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobResponsibilityTranslations",
                table: "JobResponsibilityTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobResponsibilities",
                table: "JobResponsibilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobMainDutyTranslations",
                table: "JobMainDutyTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobMainDuties",
                table: "JobMainDuties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobExtraBenefitTranslations",
                table: "JobExtraBenefitTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobExtraBenefits",
                table: "JobExtraBenefits");

            migrationBuilder.RenameTable(
                name: "JobResponsibilityTranslations",
                newName: "JobResponsibilityTranslation");

            migrationBuilder.RenameTable(
                name: "JobResponsibilities",
                newName: "JobResponsibility");

            migrationBuilder.RenameTable(
                name: "JobMainDutyTranslations",
                newName: "JobMainDutyTranslation");

            migrationBuilder.RenameTable(
                name: "JobMainDuties",
                newName: "JobMainDuty");

            migrationBuilder.RenameTable(
                name: "JobExtraBenefitTranslations",
                newName: "JobExtraBenefitTranslation");

            migrationBuilder.RenameTable(
                name: "JobExtraBenefits",
                newName: "JobExtraBenefit");

            migrationBuilder.RenameIndex(
                name: "IX_JobResponsibilityTranslations_LanguageId",
                table: "JobResponsibilityTranslation",
                newName: "IX_JobResponsibilityTranslation_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_JobResponsibilityTranslations_JobResponsibilityId",
                table: "JobResponsibilityTranslation",
                newName: "IX_JobResponsibilityTranslation_JobResponsibilityId");

            migrationBuilder.RenameIndex(
                name: "IX_JobResponsibilities_JobId",
                table: "JobResponsibility",
                newName: "IX_JobResponsibility_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobMainDutyTranslations_LanguageId",
                table: "JobMainDutyTranslation",
                newName: "IX_JobMainDutyTranslation_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_JobMainDutyTranslations_JobMainDutyId",
                table: "JobMainDutyTranslation",
                newName: "IX_JobMainDutyTranslation_JobMainDutyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobMainDuties_JobId",
                table: "JobMainDuty",
                newName: "IX_JobMainDuty_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobExtraBenefitTranslations_LanguageId",
                table: "JobExtraBenefitTranslation",
                newName: "IX_JobExtraBenefitTranslation_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_JobExtraBenefitTranslations_JobExtraBenefitId",
                table: "JobExtraBenefitTranslation",
                newName: "IX_JobExtraBenefitTranslation_JobExtraBenefitId");

            migrationBuilder.RenameIndex(
                name: "IX_JobExtraBenefits_JobId",
                table: "JobExtraBenefit",
                newName: "IX_JobExtraBenefit_JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobResponsibilityTranslation",
                table: "JobResponsibilityTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobResponsibility",
                table: "JobResponsibility",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobMainDutyTranslation",
                table: "JobMainDutyTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobMainDuty",
                table: "JobMainDuty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobExtraBenefitTranslation",
                table: "JobExtraBenefitTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobExtraBenefit",
                table: "JobExtraBenefit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobExtraBenefit_Jobs_JobId",
                table: "JobExtraBenefit",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobExtraBenefitTranslation_JobExtraBenefit_JobExtraBenefitId",
                table: "JobExtraBenefitTranslation",
                column: "JobExtraBenefitId",
                principalTable: "JobExtraBenefit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobExtraBenefitTranslation_Languages_LanguageId",
                table: "JobExtraBenefitTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobMainDuty_Jobs_JobId",
                table: "JobMainDuty",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobMainDutyTranslation_JobMainDuty_JobMainDutyId",
                table: "JobMainDutyTranslation",
                column: "JobMainDutyId",
                principalTable: "JobMainDuty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobMainDutyTranslation_Languages_LanguageId",
                table: "JobMainDutyTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobResponsibility_Jobs_JobId",
                table: "JobResponsibility",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobResponsibilityTranslation_JobResponsibility_JobResponsibilityId",
                table: "JobResponsibilityTranslation",
                column: "JobResponsibilityId",
                principalTable: "JobResponsibility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobResponsibilityTranslation_Languages_LanguageId",
                table: "JobResponsibilityTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

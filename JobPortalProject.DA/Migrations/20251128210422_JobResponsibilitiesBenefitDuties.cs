using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class JobResponsibilitiesBenefitDuties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraBenefits",
                table: "JobTranslations");

            migrationBuilder.DropColumn(
                name: "MainDuties",
                table: "JobTranslations");

            migrationBuilder.DropColumn(
                name: "Responsibilities",
                table: "JobTranslations");

            migrationBuilder.CreateTable(
                name: "JobExtraBenefit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExtraBenefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobExtraBenefit_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobMainDuty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobMainDuty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobMainDuty_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobResponsibility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobResponsibility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobResponsibility_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobExtraBenefitTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobExtraBenefitId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExtraBenefitTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobExtraBenefitTranslation_JobExtraBenefit_JobExtraBenefitId",
                        column: x => x.JobExtraBenefitId,
                        principalTable: "JobExtraBenefit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobExtraBenefitTranslation_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobMainDutyTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobMainDutyId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobMainDutyTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobMainDutyTranslation_JobMainDuty_JobMainDutyId",
                        column: x => x.JobMainDutyId,
                        principalTable: "JobMainDuty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobMainDutyTranslation_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobResponsibilityTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobResponsibilityId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobResponsibilityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobResponsibilityTranslation_JobResponsibility_JobResponsibilityId",
                        column: x => x.JobResponsibilityId,
                        principalTable: "JobResponsibility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobResponsibilityTranslation_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobExtraBenefit_JobId",
                table: "JobExtraBenefit",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobExtraBenefitTranslation_JobExtraBenefitId",
                table: "JobExtraBenefitTranslation",
                column: "JobExtraBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_JobExtraBenefitTranslation_LanguageId",
                table: "JobExtraBenefitTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_JobMainDuty_JobId",
                table: "JobMainDuty",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobMainDutyTranslation_JobMainDutyId",
                table: "JobMainDutyTranslation",
                column: "JobMainDutyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobMainDutyTranslation_LanguageId",
                table: "JobMainDutyTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_JobResponsibility_JobId",
                table: "JobResponsibility",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobResponsibilityTranslation_JobResponsibilityId",
                table: "JobResponsibilityTranslation",
                column: "JobResponsibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobResponsibilityTranslation_LanguageId",
                table: "JobResponsibilityTranslation",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobExtraBenefitTranslation");

            migrationBuilder.DropTable(
                name: "JobMainDutyTranslation");

            migrationBuilder.DropTable(
                name: "JobResponsibilityTranslation");

            migrationBuilder.DropTable(
                name: "JobExtraBenefit");

            migrationBuilder.DropTable(
                name: "JobMainDuty");

            migrationBuilder.DropTable(
                name: "JobResponsibility");

            migrationBuilder.AddColumn<string>(
                name: "ExtraBenefits",
                table: "JobTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainDuties",
                table: "JobTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Responsibilities",
                table: "JobTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

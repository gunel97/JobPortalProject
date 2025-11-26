using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalProject.DA.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingField_Companies_CompanyId",
                table: "WorkingField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingField",
                table: "WorkingField");

            migrationBuilder.RenameTable(
                name: "WorkingField",
                newName: "WorkingFields");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingField_CompanyId",
                table: "WorkingFields",
                newName: "IX_WorkingFields_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingFields",
                table: "WorkingFields",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WorkingFieldTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkingFieldId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingFieldTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingFieldTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingFieldTranslations_WorkingFields_WorkingFieldId",
                        column: x => x.WorkingFieldId,
                        principalTable: "WorkingFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingFieldTranslations_LanguageId",
                table: "WorkingFieldTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingFieldTranslations_WorkingFieldId",
                table: "WorkingFieldTranslations",
                column: "WorkingFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingFields_Companies_CompanyId",
                table: "WorkingFields",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingFields_Companies_CompanyId",
                table: "WorkingFields");

            migrationBuilder.DropTable(
                name: "WorkingFieldTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingFields",
                table: "WorkingFields");

            migrationBuilder.RenameTable(
                name: "WorkingFields",
                newName: "WorkingField");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingFields_CompanyId",
                table: "WorkingField",
                newName: "IX_WorkingField_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingField",
                table: "WorkingField",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingField_Companies_CompanyId",
                table: "WorkingField",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RiskClassificationId",
                table: "FCase",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RiskClassification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrdinalNumber = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskClassification", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FCase_RiskClassificationId",
                table: "FCase",
                column: "RiskClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCase_RiskClassification_RiskClassificationId",
                table: "FCase",
                column: "RiskClassificationId",
                principalTable: "RiskClassification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCase_RiskClassification_RiskClassificationId",
                table: "FCase");

            migrationBuilder.DropTable(
                name: "RiskClassification");

            migrationBuilder.DropIndex(
                name: "IX_FCase_RiskClassificationId",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "RiskClassificationId",
                table: "FCase");
        }
    }
}

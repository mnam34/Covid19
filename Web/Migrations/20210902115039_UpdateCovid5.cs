using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CommuneId",
                table: "FCase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DistrictId",
                table: "FCase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProvinceId",
                table: "FCase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_FCase_CommuneId",
                table: "FCase",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_DistrictId",
                table: "FCase",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_ProvinceId",
                table: "FCase",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCase_Commune_CommuneId",
                table: "FCase",
                column: "CommuneId",
                principalTable: "Commune",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FCase_District_DistrictId",
                table: "FCase",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FCase_Province_ProvinceId",
                table: "FCase",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCase_Commune_CommuneId",
                table: "FCase");

            migrationBuilder.DropForeignKey(
                name: "FK_FCase_District_DistrictId",
                table: "FCase");

            migrationBuilder.DropForeignKey(
                name: "FK_FCase_Province_ProvinceId",
                table: "FCase");

            migrationBuilder.DropIndex(
                name: "IX_FCase_CommuneId",
                table: "FCase");

            migrationBuilder.DropIndex(
                name: "IX_FCase_DistrictId",
                table: "FCase");

            migrationBuilder.DropIndex(
                name: "IX_FCase_ProvinceId",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "CommuneId",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "FCase");
        }
    }
}

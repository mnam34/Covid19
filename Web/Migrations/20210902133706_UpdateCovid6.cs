using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DistrictId",
                table: "QuarantinePlace",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProvinceId",
                table: "QuarantinePlace",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DistrictId",
                table: "EpidemicArea",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProvinceId",
                table: "EpidemicArea",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_QuarantinePlace_DistrictId",
                table: "QuarantinePlace",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarantinePlace_ProvinceId",
                table: "QuarantinePlace",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_EpidemicArea_DistrictId",
                table: "EpidemicArea",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_EpidemicArea_ProvinceId",
                table: "EpidemicArea",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EpidemicArea_District_DistrictId",
                table: "EpidemicArea",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EpidemicArea_Province_ProvinceId",
                table: "EpidemicArea",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuarantinePlace_District_DistrictId",
                table: "QuarantinePlace",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuarantinePlace_Province_ProvinceId",
                table: "QuarantinePlace",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EpidemicArea_District_DistrictId",
                table: "EpidemicArea");

            migrationBuilder.DropForeignKey(
                name: "FK_EpidemicArea_Province_ProvinceId",
                table: "EpidemicArea");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarantinePlace_District_DistrictId",
                table: "QuarantinePlace");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarantinePlace_Province_ProvinceId",
                table: "QuarantinePlace");

            migrationBuilder.DropIndex(
                name: "IX_QuarantinePlace_DistrictId",
                table: "QuarantinePlace");

            migrationBuilder.DropIndex(
                name: "IX_QuarantinePlace_ProvinceId",
                table: "QuarantinePlace");

            migrationBuilder.DropIndex(
                name: "IX_EpidemicArea_DistrictId",
                table: "EpidemicArea");

            migrationBuilder.DropIndex(
                name: "IX_EpidemicArea_ProvinceId",
                table: "EpidemicArea");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "QuarantinePlace");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "QuarantinePlace");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "EpidemicArea");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "EpidemicArea");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AddressCommuneId",
                table: "FCase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AddressDistrictId",
                table: "FCase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AddressProvinceId",
                table: "FCase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressCommuneId",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "AddressDistrictId",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "AddressProvinceId",
                table: "FCase");
        }
    }
}

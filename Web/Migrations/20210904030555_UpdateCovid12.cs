using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "FCase",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfBirth",
                table: "FCase",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "YearOfBirth",
                table: "FCase");
        }
    }
}

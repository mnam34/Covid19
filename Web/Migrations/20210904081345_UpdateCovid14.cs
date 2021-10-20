using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFx2F0",
                table: "FCase",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFx2F0",
                table: "FCase");
        }
    }
}

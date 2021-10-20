using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "TreatmentFacility",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "TreatmentFacility");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Epidemiology",
                table: "FCase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FxContactDate",
                table: "FCase",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "FCase",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Epidemiology",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "FxContactDate",
                table: "FCase");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "FCase");
        }
    }
}

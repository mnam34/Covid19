using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UpdateCovid1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "QuarantinePlace");

            migrationBuilder.DropColumn(
                name: "LockdownDate",
                table: "QuarantinePlace");

            migrationBuilder.RenameColumn(
                name: "UnLockdownDate",
                table: "QuarantinePlace",
                newName: "CloseDate");

            migrationBuilder.RenameColumn(
                name: "OutbreakDate",
                table: "QuarantinePlace",
                newName: "OpenDate");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "QuarantinePlace",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "QuarantinePlace",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "QuarantinePlace",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReOpenTime",
                table: "QuarantinePlace",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "QuarantinePlace");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "QuarantinePlace");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "QuarantinePlace");

            migrationBuilder.DropColumn(
                name: "ReOpenTime",
                table: "QuarantinePlace");

            migrationBuilder.RenameColumn(
                name: "OpenDate",
                table: "QuarantinePlace",
                newName: "OutbreakDate");

            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "QuarantinePlace",
                newName: "UnLockdownDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "QuarantinePlace",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockdownDate",
                table: "QuarantinePlace",
                type: "datetime2",
                nullable: true);
        }
    }
}

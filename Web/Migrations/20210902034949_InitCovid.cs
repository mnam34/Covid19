using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class InitCovid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EpidemicArea",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockdownDate",
                table: "EpidemicArea",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OutbreakDate",
                table: "EpidemicArea",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UnLockdownDate",
                table: "EpidemicArea",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DetectedPlace",
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
                    table.PrimaryKey("PK_DetectedPlace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuarantinePlace",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OutbreakDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LockdownDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnLockdownDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommuneId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarantinePlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuarantinePlace_Commune_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Commune",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuarantineType",
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
                    table.PrimaryKey("PK_QuarantineType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentFacility",
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
                    table.PrimaryKey("PK_TreatmentFacility", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCase",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsF0 = table.Column<bool>(type: "bit", nullable: false),
                    F0Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCured = table.Column<bool>(type: "bit", nullable: false),
                    CuredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSuspected = table.Column<bool>(type: "bit", nullable: false),
                    SuspectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSafe = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmSafeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeath = table.Column<bool>(type: "bit", nullable: false),
                    DeathDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonitorStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonitorEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MovingRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EpidemicAreaId = table.Column<long>(type: "bigint", nullable: false),
                    EpidemicAreaRelatedId = table.Column<long>(type: "bigint", nullable: true),
                    DetectedPlaceId = table.Column<long>(type: "bigint", nullable: true),
                    QuarantineTypeId = table.Column<long>(type: "bigint", nullable: true),
                    QuarantineDays = table.Column<int>(type: "int", nullable: false),
                    QuarantinePlaceId = table.Column<long>(type: "bigint", nullable: true),
                    TreatmentFacilityId = table.Column<long>(type: "bigint", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    FCaseId = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCase_DetectedPlace_DetectedPlaceId",
                        column: x => x.DetectedPlaceId,
                        principalTable: "DetectedPlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FCase_EpidemicArea_EpidemicAreaId",
                        column: x => x.EpidemicAreaId,
                        principalTable: "EpidemicArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FCase_FCase_FCaseId",
                        column: x => x.FCaseId,
                        principalTable: "FCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FCase_QuarantinePlace_QuarantinePlaceId",
                        column: x => x.QuarantinePlaceId,
                        principalTable: "QuarantinePlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FCase_QuarantineType_QuarantineTypeId",
                        column: x => x.QuarantineTypeId,
                        principalTable: "QuarantineType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FCase_TreatmentFacility_TreatmentFacilityId",
                        column: x => x.TreatmentFacilityId,
                        principalTable: "TreatmentFacility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCaseDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FCaseId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCaseDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCaseDocument_FCase_FCaseId",
                        column: x => x.FCaseId,
                        principalTable: "FCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestResult",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsNegative = table.Column<bool>(type: "bit", nullable: false),
                    IsPositive = table.Column<bool>(type: "bit", nullable: false),
                    ResultDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FCaseId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResult_FCase_FCaseId",
                        column: x => x.FCaseId,
                        principalTable: "FCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FCase_DetectedPlaceId",
                table: "FCase",
                column: "DetectedPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_EpidemicAreaId",
                table: "FCase",
                column: "EpidemicAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_FCaseId",
                table: "FCase",
                column: "FCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_QuarantinePlaceId",
                table: "FCase",
                column: "QuarantinePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_QuarantineTypeId",
                table: "FCase",
                column: "QuarantineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FCase_TreatmentFacilityId",
                table: "FCase",
                column: "TreatmentFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FCaseDocument_FCaseId",
                table: "FCaseDocument",
                column: "FCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarantinePlace_CommuneId",
                table: "QuarantinePlace",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_FCaseId",
                table: "TestResult",
                column: "FCaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FCaseDocument");

            migrationBuilder.DropTable(
                name: "TestResult");

            migrationBuilder.DropTable(
                name: "FCase");

            migrationBuilder.DropTable(
                name: "DetectedPlace");

            migrationBuilder.DropTable(
                name: "QuarantinePlace");

            migrationBuilder.DropTable(
                name: "QuarantineType");

            migrationBuilder.DropTable(
                name: "TreatmentFacility");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EpidemicArea");

            migrationBuilder.DropColumn(
                name: "LockdownDate",
                table: "EpidemicArea");

            migrationBuilder.DropColumn(
                name: "OutbreakDate",
                table: "EpidemicArea");

            migrationBuilder.DropColumn(
                name: "UnLockdownDate",
                table: "EpidemicArea");
        }
    }
}

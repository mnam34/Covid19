using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class CreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessRight = table.Column<bool>(type: "bit", nullable: false),
                    RealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsManageAccount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    AuditLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventDateDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginalValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.AuditLogId);
                });

            migrationBuilder.CreateTable(
                name: "RequestLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullComputerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComputerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsoluteUri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrdinalNumber = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRole_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Create = table.Column<byte>(type: "tinyint", nullable: true),
                    Read = table.Column<byte>(type: "tinyint", nullable: true),
                    Update = table.Column<byte>(type: "tinyint", nullable: true),
                    Delete = table.Column<byte>(type: "tinyint", nullable: true),
                    Verify = table.Column<byte>(type: "tinyint", nullable: true),
                    Publish = table.Column<byte>(type: "tinyint", nullable: true),
                    Confirm = table.Column<byte>(type: "tinyint", nullable: true),
                    Approve = table.Column<byte>(type: "tinyint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_AccountId",
                table: "AccountRole",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleRole_RoleId",
                table: "ModuleRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRole");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "ModuleRole");

            migrationBuilder.DropTable(
                name: "RequestLog");

            migrationBuilder.DropTable(
                name: "SystemConfig");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}

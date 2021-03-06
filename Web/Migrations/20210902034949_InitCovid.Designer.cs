// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210902034949_InitCovid")]
    partial class InitCovid
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AccessRight")
                        .HasColumnType("bit");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("IsManageAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RealName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Entities.AccountRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRole");
                });

            modelBuilder.Entity("Entities.AuditLog", b =>
                {
                    b.Property<Guid>("AuditLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("ColumnName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventDateDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecordKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AuditLogId");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("Entities.Commune", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<long>("DistrictId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<bool>("Unused")
                        .HasColumnType("bit");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Commune");
                });

            modelBuilder.Entity("Entities.DetectedPlace", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DetectedPlace");
                });

            modelBuilder.Entity("Entities.District", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<long>("ProvinceId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Unused")
                        .HasColumnType("bit");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("District");
                });

            modelBuilder.Entity("Entities.EpidemicArea", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CommuneId")
                        .HasColumnType("bigint");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LockdownDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("OutbreakDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UnLockdownDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("CommuneId");

                    b.ToTable("EpidemicArea");
                });

            modelBuilder.Entity("Entities.FCase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime?>("ConfirmSafeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ContactHistory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CuredDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeathDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("DetectedPlaceId")
                        .HasColumnType("bigint");

                    b.Property<long>("EpidemicAreaId")
                        .HasColumnType("bigint");

                    b.Property<long?>("EpidemicAreaRelatedId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("F0Date")
                        .HasColumnType("datetime2");

                    b.Property<long?>("FCaseId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsCured")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeath")
                        .HasColumnType("bit");

                    b.Property<bool>("IsF0")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSafe")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSuspected")
                        .HasColumnType("bit");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<DateTime?>("MonitorEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("MonitorStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MovingRoute")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("QuarantineDays")
                        .HasColumnType("int");

                    b.Property<long?>("QuarantinePlaceId")
                        .HasColumnType("bigint");

                    b.Property<long?>("QuarantineTypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("SuspectedDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("TreatmentFacilityId")
                        .HasColumnType("bigint");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DetectedPlaceId");

                    b.HasIndex("EpidemicAreaId");

                    b.HasIndex("FCaseId");

                    b.HasIndex("QuarantinePlaceId");

                    b.HasIndex("QuarantineTypeId");

                    b.HasIndex("TreatmentFacilityId");

                    b.ToTable("FCase");
                });

            modelBuilder.Entity("Entities.FCaseDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DocumentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocumentPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FCaseId")
                        .HasColumnType("bigint");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FCaseId");

                    b.ToTable("FCaseDocument");
                });

            modelBuilder.Entity("Entities.ModuleRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte?>("Approve")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Confirm")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Create")
                        .HasColumnType("tinyint");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<byte?>("Delete")
                        .HasColumnType("tinyint");

                    b.Property<string>("ModuleCode")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte?>("Publish")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Read")
                        .HasColumnType("tinyint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<byte?>("Update")
                        .HasColumnType("tinyint");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<byte?>("Verify")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("ModuleRole");
                });

            modelBuilder.Entity("Entities.Province", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<bool>("Unused")
                        .HasColumnType("bit");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.ToTable("Province");
                });

            modelBuilder.Entity("Entities.QuarantinePlace", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CommuneId")
                        .HasColumnType("bigint");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LockdownDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("OutbreakDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UnLockdownDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CommuneId");

                    b.ToTable("QuarantinePlace");
                });

            modelBuilder.Entity("Entities.QuarantineType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("QuarantineType");
                });

            modelBuilder.Entity("Entities.RequestLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AbsoluteUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrowserInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ComputerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("FullComputerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RawUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RemoteIpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RequestLog");
                });

            modelBuilder.Entity("Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Entities.SystemConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemConfig");
                });

            modelBuilder.Entity("Entities.TestResult", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("FCaseId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsNegative")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ResultDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResultDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TestDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FCaseId");

                    b.ToTable("TestResult");
                });

            modelBuilder.Entity("Entities.TreatmentFacility", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("OrdinalNumber")
                        .HasColumnType("int");

                    b.Property<long>("UpdateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TreatmentFacility");
                });

            modelBuilder.Entity("Entities.AccountRole", b =>
                {
                    b.HasOne("Entities.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Entities.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Entities.Commune", b =>
                {
                    b.HasOne("Entities.District", "District")
                        .WithMany("Communes")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Entities.District", b =>
                {
                    b.HasOne("Entities.Province", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Entities.EpidemicArea", b =>
                {
                    b.HasOne("Entities.Commune", "Commune")
                        .WithMany("EpidemicAreas")
                        .HasForeignKey("CommuneId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Commune");
                });

            modelBuilder.Entity("Entities.FCase", b =>
                {
                    b.HasOne("Entities.DetectedPlace", "DetectedPlace")
                        .WithMany("FCases")
                        .HasForeignKey("DetectedPlaceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.EpidemicArea", "EpidemicArea")
                        .WithMany("FCases")
                        .HasForeignKey("EpidemicAreaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Entities.FCase", "ParentFCase")
                        .WithMany("FCases")
                        .HasForeignKey("FCaseId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.QuarantinePlace", "QuarantinePlace")
                        .WithMany("FCases")
                        .HasForeignKey("QuarantinePlaceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.QuarantineType", "QuarantineType")
                        .WithMany("FCases")
                        .HasForeignKey("QuarantineTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.TreatmentFacility", "TreatmentFacility")
                        .WithMany("FCases")
                        .HasForeignKey("TreatmentFacilityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("DetectedPlace");

                    b.Navigation("EpidemicArea");

                    b.Navigation("ParentFCase");

                    b.Navigation("QuarantinePlace");

                    b.Navigation("QuarantineType");

                    b.Navigation("TreatmentFacility");
                });

            modelBuilder.Entity("Entities.FCaseDocument", b =>
                {
                    b.HasOne("Entities.FCase", "FCase")
                        .WithMany("FCaseDocuments")
                        .HasForeignKey("FCaseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FCase");
                });

            modelBuilder.Entity("Entities.ModuleRole", b =>
                {
                    b.HasOne("Entities.Role", "Role")
                        .WithMany("ModuleRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Entities.QuarantinePlace", b =>
                {
                    b.HasOne("Entities.Commune", "Commune")
                        .WithMany("QuarantinePlaces")
                        .HasForeignKey("CommuneId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Commune");
                });

            modelBuilder.Entity("Entities.TestResult", b =>
                {
                    b.HasOne("Entities.FCase", "FCase")
                        .WithMany("TestResults")
                        .HasForeignKey("FCaseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FCase");
                });

            modelBuilder.Entity("Entities.Account", b =>
                {
                    b.Navigation("AccountRoles");
                });

            modelBuilder.Entity("Entities.Commune", b =>
                {
                    b.Navigation("EpidemicAreas");

                    b.Navigation("QuarantinePlaces");
                });

            modelBuilder.Entity("Entities.DetectedPlace", b =>
                {
                    b.Navigation("FCases");
                });

            modelBuilder.Entity("Entities.District", b =>
                {
                    b.Navigation("Communes");
                });

            modelBuilder.Entity("Entities.EpidemicArea", b =>
                {
                    b.Navigation("FCases");
                });

            modelBuilder.Entity("Entities.FCase", b =>
                {
                    b.Navigation("FCaseDocuments");

                    b.Navigation("FCases");

                    b.Navigation("TestResults");
                });

            modelBuilder.Entity("Entities.Province", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("Entities.QuarantinePlace", b =>
                {
                    b.Navigation("FCases");
                });

            modelBuilder.Entity("Entities.QuarantineType", b =>
                {
                    b.Navigation("FCases");
                });

            modelBuilder.Entity("Entities.Role", b =>
                {
                    b.Navigation("AccountRoles");

                    b.Navigation("ModuleRoles");
                });

            modelBuilder.Entity("Entities.TreatmentFacility", b =>
                {
                    b.Navigation("FCases");
                });
#pragma warning restore 612, 618
        }
    }
}

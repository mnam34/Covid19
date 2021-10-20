using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>().Property(x => x.IsManageAccount).HasDefaultValue(false);
            modelBuilder.Entity<Account>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Account>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<AccountRole>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<AccountRole>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ModuleRole>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ModuleRole>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Role>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Role>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<SystemConfig>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<SystemConfig>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<RequestLog>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Province>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Province>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<District>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<District>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Commune>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Commune>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<EpidemicArea>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<EpidemicArea>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<DetectedPlace>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<DetectedPlace>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<FCase>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<FCase>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<FCaseDocument>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<FCaseDocument>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<QuarantinePlace>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<QuarantinePlace>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<QuarantineType>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<QuarantineType>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TestResult>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TestResult>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TreatmentFacility>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TreatmentFacility>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<RiskClassification>().Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<RiskClassification>().Property(x => x.UpdateDate).HasDefaultValueSql("getdate()");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entity.Name).ToTable(entity.Name.Replace("Entities.", string.Empty));
            }
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            ConfigureModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        private void ConfigureModel(ModelBuilder modelBuilder)
        {
            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
            var entityTypes = typeof(Entity).GetTypeInfo().Assembly.GetTypes().Where(x => x.Name != "Entity" && (x.Namespace == "Entities" || x.Namespace == "Entities.Models" || x.Namespace == "Entities.Models.SystemManage") && x.FullName != "Entities.IEntity" && x.FullName != "Entities.Entity").ToList();
            foreach (var type in entityTypes)
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
                modelBuilder.Entity(type);
            }
        }
    }
}

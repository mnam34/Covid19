using Common.Helpers;
using Entities;
using Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> dbSet;
        private readonly DataContext _dbContext;
        public GenericRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<TEntity>();
        }
        public GenericRepository()
        {
        }
        //#################
        public IQueryable<TEntity> GetAll(ref Paging paging, string orderKey, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            var source = dbSet.Where(predicate);
            IQueryable<TEntity> resultWithEagerLoading = func(source);

            var type = typeof(TEntity);
            var property = type.GetProperty(orderKey);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), paging.OrderDirection == "asc" ? "OrderBy" : "OrderByDescending", new Type[] { type, property.PropertyType },
                resultWithEagerLoading.Expression, Expression.Quote(orderByExp));
            paging.TotalRecord = resultWithEagerLoading.Count();
            if (paging.Take != -1)
                return resultWithEagerLoading.Provider.CreateQuery<TEntity>(resultExp).Skip(paging.Skip).Take(paging.Take);
            else
                return resultWithEagerLoading.Provider.CreateQuery<TEntity>(resultExp).Skip(paging.Skip);
        }
        //GetAll
        public IQueryable<TEntity> GetAll(ref Paging paging, Expression<Func<TEntity, string>> orderKey, Expression<Func<TEntity, bool>> predicate)
        {
            paging.TotalRecord = this.Count(predicate);
            var rusult = dbSet.Where(predicate).OrderBy(orderKey).Skip(paging.Skip);
            if (paging.Take != -1)
                return rusult.Take(paging.Take);
            else
                return rusult;
        }

        public IQueryable<TEntity> GetAll(ref Paging paging, string orderKey, Expression<Func<TEntity, bool>> predicate)
        {
            var source = dbSet.Where(predicate);
            var type = typeof(TEntity);
            var property = type.GetProperty(orderKey);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), paging.OrderDirection == "asc" ? "OrderBy" : "OrderByDescending", new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExp));
            paging.TotalRecord = source.Count();
            if (paging.Take != -1)
                return source.Provider.CreateQuery<TEntity>(resultExp).Skip(paging.Skip).Take(paging.Take);
            else
                return source.Provider.CreateQuery<TEntity>(resultExp).Skip(paging.Skip);
        }
        public IQueryable<TEntity> GetAll<TKey>(ref Paging paging, Expression<Func<TEntity, TKey>> orderKey, Expression<Func<TEntity, bool>> predicate)
        {
            var source = dbSet.Where(predicate);
            paging.TotalRecord = source.Count();
            IQueryable<TEntity> result;
            if (paging.OrderDirection == "asc" || paging.OrderDirection == "OrderBy")
            {
                result = source.OrderBy(orderKey);
            }
            else
            {
                result = source.OrderByDescending(orderKey);
            }
            if (paging.Take != -1)
                return result.Skip(paging.Skip).Take(paging.Take);
            else
                return result.Skip(paging.Skip);
        }
        public IQueryable<TEntity> GetAll<TKey>(ref Paging paging, Expression<Func<TEntity, TKey>> orderKey, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            var source = dbSet.Where(predicate);
            IQueryable<TEntity> resultWithEagerLoading = func(source);
            paging.TotalRecord = resultWithEagerLoading.Count();
            IQueryable<TEntity> result;
            if (paging.OrderDirection == "asc" || paging.OrderDirection == "OrderBy")
            {
                result = resultWithEagerLoading.OrderBy(orderKey);
            }
            else
            {
                result = resultWithEagerLoading.OrderByDescending(orderKey);
            }
            if (paging.Take != -1)
                return result.Skip(paging.Skip).Take(paging.Take);
            else
                return result.Skip(paging.Skip);
        }
        public IQueryable<TEntity> GetAll<TKey>(ref Paging paging, Expression<Func<TEntity, TEntity>> keySelectors, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderKey)
        {
            var source = dbSet.Where(predicate);
            paging.TotalRecord = source.Count();
            if (paging.Take != -1)
                return source.Select(keySelectors).OrderBy(orderKey).Skip(paging.Skip).Take(paging.Take);
            else
                return source.Select(keySelectors).OrderBy(orderKey).Skip(paging.Skip);
        }


        public IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            IQueryable<TEntity> resultWithEagerLoading = func(dbSet);
            return resultWithEagerLoading.Where(predicate);
        }
        //End GetAll
        //#################
        //GetAllAsync
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            IQueryable<TEntity> resultWithEagerLoading = func(dbSet);
            return await resultWithEagerLoading.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            IQueryable<TEntity> resultWithEagerLoading = func(dbSet);
            return await resultWithEagerLoading.Where(predicate).ToListAsync();
        }
        //End GetAllAsync
        //Read #################

        public TEntity Read(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }
        public TEntity Read(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            IQueryable<TEntity> resultWithEagerLoading = func(dbSet);
            return resultWithEagerLoading.FirstOrDefault(predicate);
        }
        public TEntity Read(long id)
        {
            return dbSet.Find(id);
        }
        public async Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            IQueryable<TEntity> resultWithEagerLoading = func(dbSet);
            return await resultWithEagerLoading.FirstOrDefaultAsync(predicate);
        }
        public async Task<TEntity> ReadAsync(long id)
        {
            return await dbSet.FindAsync(id);
        }
        //Update #################
        public int Update(TEntity entity, long accountId)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return _dbContext.SaveChanges();
            return this.SaveChanges(accountId, time);
        }
        public async Task<int> UpdateAsync(TEntity entity, long accountId)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return await _dbContext.SaveChangesAsync();
            return await this.SaveChangesAsync(accountId, time);
        }
        //Create #################
        public int Create(TEntity entity, long accountId)
        {
            dbSet.Add(entity);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return _dbContext.SaveChanges();
            return this.SaveChanges(accountId, time);
        }
        public async Task<int> CreateAsync(TEntity entity, long accountId)
        {
            dbSet.Add(entity);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return await _dbContext.SaveChangesAsync();
            return await this.SaveChangesAsync(accountId, time);
        }
        public int Create(IEnumerable<TEntity> entities, long accountId)
        {
            dbSet.AddRange(entities);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return _dbContext.SaveChanges();
            return this.SaveChanges(accountId, time);
        }
        public async Task<int> CreateAsync(IEnumerable<TEntity> entities, long accountId)
        {
            dbSet.AddRange(entities);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return await _dbContext.SaveChangesAsync();
            return await this.SaveChangesAsync(accountId, time);
        }
        //Delete #################
        public int Delete(TEntity entity, long accountId)
        {
            dbSet.Remove(entity);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return _dbContext.SaveChanges();
            return this.SaveChanges(accountId, time);
        }

        //public void DeleteItem(TEntity entity)
        //{
        //    DbSet.Remove(entity);
        //}
        public int Delete(Expression<Func<TEntity, bool>> predicate, long accountId)
        {
            var entities = GetAll(predicate);
            //foreach (TEntity entity in entities.ToList<TEntity>())
            //{
            //    DeleteItem(entity);
            //}
            dbSet.RemoveRange(entities);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return _dbContext.SaveChanges();
            return this.SaveChanges(accountId, time);
        }
        public async Task<int> DeleteAsync(TEntity entity, long accountId)
        {
            dbSet.Remove(entity);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return await _dbContext.SaveChangesAsync();
            return await this.SaveChangesAsync(accountId, time);
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, long accountId)
        {
            var entities = GetAll(predicate);
            //foreach (TEntity entity in entities.ToList<TEntity>())
            //{
            //    DeleteItem(entity);
            //}
            dbSet.RemoveRange(entities);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return await _dbContext.SaveChangesAsync();
            return await this.SaveChangesAsync(accountId, time);
        }
        public int Delete(IEnumerable<TEntity> entities, long accountId)
        {
            dbSet.RemoveRange(entities);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return _dbContext.SaveChanges();
            return this.SaveChanges(accountId, time);
        }
        public async Task<int> DeleteAsync(IEnumerable<TEntity> entities, long accountId)
        {
            dbSet.RemoveRange(entities);
            string time = String.Format("{0:dd_MM_yyyy_hh_mm_ss_fff}", DateTime.Now);
            //return await _dbContext.SaveChangesAsync();
            return await this.SaveChangesAsync(accountId, time);
        }
        //Any #################
        public bool Any()
        {
            return dbSet.Any();
        }
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }
        public async Task<bool> AnyAsync()
        {
            return await dbSet.AnyAsync();
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }
        //Count #################
        public int Count()
        {
            return dbSet.Count();
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Count(predicate);
        }
        public async Task<int> CountAsync()
        {
            return await dbSet.CountAsync();
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.CountAsync(predicate);
        }
        //Max #################
        public long Max(Expression<Func<TEntity, long>> selector)
        {
            try
            {
                if (dbSet.Any())
                    return dbSet.Max(selector);
                else
                    return 0;
            }
            catch
            {
                return long.MinValue;
            }
        }
        public async Task<long> MaxAsync(Expression<Func<TEntity, long>> selector)
        {
            try
            {
                if (await dbSet.AnyAsync())
                    return await dbSet.MaxAsync(selector);
                else
                    return 0;
            }
            catch
            {
                return long.MinValue;
            }
        }
        public string Max(Expression<Func<TEntity, string>> selector)
        {
            return dbSet.Max(selector);
        }
        //SqlQuery #################


        private int SaveChanges(long accountId, string time)
        {
            int result = 0;
            int result2 = 0;
            foreach (var ent in _dbContext.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified).ToList())
            {
                DbSet<AuditLog> DbSetLog = _dbContext.Set<AuditLog>();
                if (ent.State == EntityState.Added)
                {
                    result += _dbContext.SaveChanges();
                    DbSetLog.AddRange(GetAuditRecordsForChange(ent, accountId, time, true));
                    result2 += _dbContext.SaveChanges();
                }
                else
                {
                    DbSetLog.AddRange(GetAuditRecordsForChange(ent, accountId, time));
                    result2 += _dbContext.SaveChanges();
                }
            }

            if (result == 0) result = result2;
            return result;
        }
        private async Task<int> SaveChangesAsync(long accountId, string time)
        {
            int result = 0;
            int result2 = 0;
            foreach (var ent in _dbContext.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified).ToList())
            {
                DbSet<AuditLog> DbSetLog = _dbContext.Set<AuditLog>();
                if (ent.State == EntityState.Added)
                {
                    result += await _dbContext.SaveChangesAsync();
                    DbSetLog.AddRange(GetAuditRecordsForChange(ent, accountId, time, true));
                    if (DbSetLog.Any())
                        result2 += await _dbContext.SaveChangesAsync();
                }
                else
                {
                    DbSetLog.AddRange(GetAuditRecordsForChange(ent, accountId, time));
                    result2 += await _dbContext.SaveChangesAsync();
                }
            }
            if (result == 0) result = result2;
            return result;
        }
        private List<AuditLog> GetAuditRecordsForChange(EntityEntry dbEntry, long accountId, string time, bool create = false)
        {
            List<AuditLog> result = new List<AuditLog>();
            DateTime changeTime = DateTime.Now;
            // Lấy Table() attribute nếu có
            //TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            // Lấy Table Name thông qua Table Attr, nếu không có thì lấy theo tên lớp class
            string tmp = dbEntry.Entity.GetType().Name;
            string tableName = tmp.Split('_')[0];
            //Lấy tên của khóa chính

            string keyName = "Id";
            try
            {
                keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;
            }
            catch
            {
                keyName = "-1";
            }
            if ((dbEntry.State == EntityState.Added || create) && tableName != "RequestLog")
            {
                var RecordKey = (dbEntry.CurrentValues.ToObject() is Entity) ? (dbEntry.CurrentValues.ToObject() as Entity).Id.ToString() : "";
                result.Add(new AuditLog()
                {
                    AuditLogId = Guid.NewGuid(),
                    AccountId = accountId,
                    EventDate = changeTime,
                    EventType = "C", //C = Create = Tạo mới
                    TableName = tableName,
                    //RecordKey = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                    RecordKey = RecordKey,
                    ColumnName = "*ALL",
                    //NewValue = (dbEntry.CurrentValues.ToObject() is Entity) ? (dbEntry.CurrentValues.ToObject() as Entity).Id.ToString() : dbEntry.CurrentValues.ToObject().ToString(),
                    NewValue = (dbEntry.CurrentValues.ToObject() is Entity) ? (dbEntry.CurrentValues.ToObject() as Entity).Describe : dbEntry.CurrentValues.ToObject().ToString(),
                    EventDateDetail = time
                });
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                var RecordKey = (dbEntry.CurrentValues.ToObject() is Entity) ? (dbEntry.CurrentValues.ToObject() as Entity).Id.ToString() : "";
                result.Add(new AuditLog()
                {
                    AuditLogId = Guid.NewGuid(),
                    AccountId = accountId,
                    EventDate = changeTime,
                    EventType = "D", //D = Deleted = Xóa
                    TableName = tableName,
                    //RecordKey = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    RecordKey = RecordKey,
                    ColumnName = "*ALL",
                    //NewValue = (dbEntry.OriginalValues.ToObject() is Entity) ? (dbEntry.OriginalValues.ToObject() as Entity).Id.ToString() : dbEntry.OriginalValues.ToObject().ToString(),
                    NewValue = (dbEntry.OriginalValues.ToObject() is Entity) ? (dbEntry.OriginalValues.ToObject() as Entity).Describe : dbEntry.OriginalValues.ToObject().ToString(),
                    EventDateDetail = time
                });
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                var RecordKey = (dbEntry.CurrentValues.ToObject() is Entity) ? (dbEntry.CurrentValues.ToObject() as Entity).Id.ToString() : "";

                foreach (var property in dbEntry.Metadata.GetProperties().Where(p => p.Name != "Describe"))
                {
                    var originalValue = dbEntry.Property(property.Name).OriginalValue;
                    var currentValue = dbEntry.Property(property.Name).CurrentValue;
                    if (!object.Equals(originalValue, currentValue))
                    {
                        result.Add(new AuditLog()
                        {
                            AuditLogId = Guid.NewGuid(),
                            AccountId = accountId,
                            EventDate = changeTime,
                            EventType = "U", // U = Update = Cập nhật
                            TableName = tableName,
                            //RecordKey = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                            RecordKey = RecordKey,
                            ColumnName = property.Name,
                            OriginalValue = originalValue == null ? "" : originalValue.ToString(),
                            NewValue = currentValue == null ? "" : currentValue.ToString(),
                            EventDateDetail = time
                        });
                    }
                }
            }
            return result;
        }

    }
}

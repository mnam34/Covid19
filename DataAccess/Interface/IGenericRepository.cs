using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Interface
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> GetAll(ref Paging paging, string orderKey, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        IQueryable<TEntity> GetAll(ref Paging paging, Expression<Func<TEntity, string>> orderKey, Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(ref Paging paging, string orderKey, Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll<TKey>(ref Paging paging, Expression<Func<TEntity, TKey>> orderKey, Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll<TKey>(ref Paging paging, Expression<Func<TEntity, TKey>> orderKey, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        IQueryable<TEntity> GetAll<TKey>(ref Paging paging, Expression<Func<TEntity, TEntity>> keySelectors, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderKey);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);


        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);


        TEntity Read(Expression<Func<TEntity, bool>> predicate);
        TEntity Read(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        TEntity Read(long id);
        Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        Task<TEntity> ReadAsync(long id);

        int Update(TEntity entity, long accountId);
        Task<int> UpdateAsync(TEntity entity, long accountId);

        int Create(TEntity entity, long accountId);
        Task<int> CreateAsync(TEntity entity, long accountId);
        int Create(IEnumerable<TEntity> entities, long accountId);
        Task<int> CreateAsync(IEnumerable<TEntity> entities, long accountId);

        int Delete(TEntity entity, long accountId);

        int Delete(Expression<Func<TEntity, bool>> predicate, long accountId);
        Task<int> DeleteAsync(TEntity entity, long accountId);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, long accountId);
        int Delete(IEnumerable<TEntity> entities, long accountId);
        Task<int> DeleteAsync(IEnumerable<TEntity> entities, long accountId);

        bool Any();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        long Max(Expression<Func<TEntity, long>> selector);
        Task<long> MaxAsync(Expression<Func<TEntity, long>> selector);
        string Max(Expression<Func<TEntity, string>> selector);

    }
}

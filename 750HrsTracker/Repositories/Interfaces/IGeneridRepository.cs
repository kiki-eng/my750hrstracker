using System.Linq.Expressions;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetSingleOrDefaultAsync(Guid id);
        Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}

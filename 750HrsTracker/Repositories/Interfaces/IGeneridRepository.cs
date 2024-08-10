using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using System.Linq.Expressions;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetSingleOrDefaultAsync(Guid id);
        Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SoftDeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task<RepositoryResponseHandler<TEntity>> GetAllPaginatedAsync(PaginationFilter filter, Expression<Func<TEntity, object>> orderByDescending = null!, params
            Expression<Func<TEntity, object>>[]? includeProperties);
        Task<RepositoryResponseHandler<TEntity>> GetAllPaginatedAsync(Expression<Func<TEntity, bool>> predicate, PaginationFilter filter, 
            Expression<Func<TEntity, object>> orderByDescending = null!, params Expression<Func<TEntity, object>>[]? includeProperties);

        Task<List<TEntity>> SearchEntityAsync(Expression<Func<TEntity, bool>> predicate);
        bool AllExistAsync(Expression<Func<TEntity, bool>> predicate, List<Guid> entityIds, string idColumnName);
        bool AllExistAsync(List<Guid> entityIds, string idColumnName);
    }
}

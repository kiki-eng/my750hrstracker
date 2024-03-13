using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _750HrsTracker.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();  
            return entry.Entity; 
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();  
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _context.Set<TEntity>().Where(predicate);
            return await Task.Run(() => query);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            var query = _context.Set<TEntity>();
            return await Task.Run(() => query);
        } 
        
        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _context.Set<TEntity>().Where(predicate);
            return await Task.Run(() => query);
        }
        public async Task<RepositoryResponseHandler<TEntity>> GetAllPaginatedAsync(PaginationFilter filter)
        {
            var records = await _context.Set<TEntity>().Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalCount = await _context.Set<TEntity>().CountAsync();

            RepositoryResponseHandler<TEntity> response = new RepositoryResponseHandler<TEntity>
            {
                Records = records,
                TotalCount = totalCount
            };

            return response;
        }
        
        public async Task<RepositoryResponseHandler<TEntity>> GetAllPaginatedAsync(Expression<Func<TEntity, bool>> predicate, PaginationFilter filter)
        {
            var records = await _context.Set<TEntity>().Where(predicate).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalCount = await _context.Set<TEntity>().Where(predicate).CountAsync();

            RepositoryResponseHandler<TEntity> response = new RepositoryResponseHandler<TEntity>
            {
                Records = records,
                TotalCount = totalCount
            };

            return response;
        }        
        
        public async Task<TEntity> GetSingleOrDefaultAsync(Guid id)
        {
            var data = await _context.Set<TEntity>().FindAsync(id);
            return data!;
        }

        public async Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var data = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return data!;
        }

        public async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> predicate)
            => await _context.Set<TEntity>().AnyAsync(predicate);

        public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entry = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate) ?? throw new KeyNotFoundException("Record to be deleted not found");                
            _context.Set<TEntity>().Remove(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<List<TEntity>> SearchEntityAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public bool AllExistAsync(Expression<Func<TEntity, bool>> predicate, List<Guid> entityIds, string idColumnName)
        {
            bool allExist = _context.Set<TEntity>().Where(predicate).Select(GetIdSelector(idColumnName)).All(id => entityIds.Contains(id));

            return allExist;
        }
        
        public bool AllExistAsync(List<Guid> entityIds, string idColumnName)
        {
            bool allExist = _context.Set<TEntity>().Select(GetIdSelector(idColumnName)).All(id => entityIds.Contains(id));

            return allExist;
        }

        private static Func<TEntity, Guid> GetIdSelector(string columnName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, columnName);
            var lambda = Expression.Lambda<Func<TEntity, Guid>>(property, parameter);
            return lambda.Compile();
        }
    }
}

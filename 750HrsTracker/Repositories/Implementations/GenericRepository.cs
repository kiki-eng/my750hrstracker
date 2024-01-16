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

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
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
    }
}

using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;

namespace _750HrsTracker.Repositories.Implementations
{
    public class Wrapper : IWrapper
    {
        private readonly AppDbContext _context;
        public Wrapper(AppDbContext context)
        {
            _context = context;
        }

        #region Repositroy section
        public IPropertyRepository PropertyRepository => new PropertyRepository(_context);

        #endregion

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    if (_context != null)
                        _context.Dispose();

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

using _750HrsTracker.Models;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;

namespace _750HrsTracker.Repositories.Implementations
{
    public class PropertyRepository : GenericRepository<AvailableProperty>, IPropertyRepository
    {
        private readonly AppDbContext _context;
        public PropertyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

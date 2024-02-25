using _750HrsTracker.Enums;
using _750HrsTracker.Models;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;

namespace _750HrsTracker.Repositories.Implementations
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}

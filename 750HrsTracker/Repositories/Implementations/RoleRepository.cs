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

        public async Task<Role> UpdateAsync(Guid id, Role role)
        {
            var existingRole = _context.Roles.FirstOrDefault(r => r.Id == id) ?? throw new KeyNotFoundException("Role not found");

            existingRole.Name = role.Name;
            existingRole.Slug = role.Slug;

            var updated = _context.Roles.Update(existingRole);

            await _context.SaveChangesAsync();

            return updated.Entity;

        }
    }
}

using _750HrsTracker.Models;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly AppDbContext _context;
        public PermissionRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Permission> UpdateAsync(Guid id, Permission permission)
        {
            var existingPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Permission not found");

            existingPermission.Name = permission.Name;
            existingPermission.Slug = permission.Slug;
            existingPermission.Value = permission.Value;
            existingPermission.Module = permission.Module;
            existingPermission.ModifiedAt = permission.ModifiedAt;

            var updated = _context.Permissions.Update(existingPermission);

            await _context.SaveChangesAsync();

            return updated.Entity;
            
        }
    }
}

using _750HrsTracker.Models;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<Permission> UpdateAsync(Guid id, Permission permission);    
    }
}

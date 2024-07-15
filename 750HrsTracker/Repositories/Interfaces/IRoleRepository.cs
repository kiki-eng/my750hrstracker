using _750HrsTracker.Models;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> UpdateAsync(Guid id, Role role);
    }
}

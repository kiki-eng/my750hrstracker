using _750HrsTracker.Filters;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<AvailableProperty>
    {
        Task<AvailableProperty> UpdateAsync(Guid id, Guid teamId, AvailableProperty property);
        Task<bool> AssignPropertyToUsersAsync(Guid propertyId, Guid teamId, List<Guid> userIds);
    }
}

using _750HrsTracker.Filters;
using _750HrsTracker.Models;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class PropertyRepository : GenericRepository<AvailableProperty>, IPropertyRepository
    {
        private readonly AppDbContext _context;
        public PropertyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AssignPropertyToUsersAsync(Guid propertyId, Guid teamId, List<Guid> userIds)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == propertyId && p.TeamId == teamId) ?? throw new KeyNotFoundException("Property not found");

            List<PropertyTeamUser> propertyTeamUsers = new();

            foreach (var user in userIds)
            {
                var exist = await _context.Team_User.AnyAsync(tu => tu.UserId == user);

                if (!exist)
                {
                    throw new ApplicationException("One or more users cannot access property");
                }

                PropertyTeamUser propertyTeamUser = new()
                {
                    UserId = user,
                    PropertyId = property.Id,
                    TeamId = teamId,
                };

                propertyTeamUsers.Add(propertyTeamUser);

            }

            await _context.PropertyTeamUsers.AddRangeAsync(propertyTeamUsers);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AvailableProperty> UpdateAsync(Guid id, Guid teamId, AvailableProperty property)
        {
            var existingProperty = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id && p.TeamId == teamId) ?? throw new KeyNotFoundException("Property not found");

            existingProperty.Name = property.Name;
            existingProperty.Address = property.Address;
            existingProperty.Longitude = property.Longitude;
            existingProperty.Latitude = property.Latitude;
            existingProperty.Alias = property.Alias;
            existingProperty.ModifiedAt = DateTime.Now;

            var updated = _context.Properties.Update(existingProperty);

            await _context.SaveChangesAsync();

            return updated.Entity;
                
        }
    }
}

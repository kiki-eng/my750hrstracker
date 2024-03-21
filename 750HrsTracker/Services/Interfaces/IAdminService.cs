using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IAdminService
    {
        Task<PagedResponseHandler<List<GetTeamResponse>>> GetAllTeamsAsync(PaginationFilter filter, HttpRequest httpRequest);
    }
}

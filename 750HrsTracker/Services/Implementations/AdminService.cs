using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Routing;

namespace _750HrsTracker.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private IMapper _mapper;
        private ITeamRepository _teamRepository;
        private IUriService _uriService;
        public AdminService(IMapper mapper, ITeamRepository teamRepository, IUriService uriService)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _uriService = uriService;
        }

        public async Task<PagedResponseHandler<List<GetTeamResponse>>> GetAllTeamsAsync(PaginationFilter filter, HttpRequest httpRequest)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var teams = await _teamRepository.GetAllPaginatedAsync(validFilters);

            var pagedData = (teams.Records!.Select(sn => _mapper.Map<GetTeamResponse>(sn))).ToList();
            PagedResponseHandler<List<GetTeamResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, teams.TotalCount, _uriService, httpRequest.Path);

            response.Success = true;
            response.Message = "All teams retrieved successfully";
            return response;

        }
    }
}

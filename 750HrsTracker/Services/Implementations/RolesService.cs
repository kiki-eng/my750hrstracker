using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;

namespace _750HrsTracker.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        public RolesService(IRoleRepository roleRepository, IUriService uriService, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _uriService = uriService;
            _mapper = mapper;
        }

        public async Task<ResponseHandler<string>> DeleteRoleAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var deleted = await _roleRepository.DeleteAsync(p => p.Id == id);

            response.Success = true;
            response.Message = "Role deleted successfully";

            return response;
        }

        public async Task<PagedResponseHandler<List<GetRoleResponse>>> GetAllRolesAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _roleRepository.GetAllPaginatedAsync(filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetRoleResponse>(sn))).ToList();

            PagedResponseHandler<List<GetRoleResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All roles retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<GetRoleResponse>> UpdateRoleAsync(Guid id, AddUpdateRolesRequest request)
        {
            ResponseHandler<GetRoleResponse> response = new();

            var role = _mapper.Map<Role>(request);
            role.Slug = role.Name.Replace(" ", "_").ToLower();

            var exists = await _roleRepository.IsAnyAsync(r => r.Name!.ToLower() == role.Name!.ToLower());
            if (exists)
            {
                throw new ApplicationException("Role already exists");
            }

            var updatedRole = await _roleRepository.UpdateAsync(id, role);

            response.Success = true;
            response.Message = "Role updated successfully";
            response.Data = _mapper.Map<GetRoleResponse>(updatedRole);

            return response;
        }
    }
}

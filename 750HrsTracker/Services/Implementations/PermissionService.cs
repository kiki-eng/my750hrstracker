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
using System.Security;

namespace _750HrsTracker.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper, IUriService uriService)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<ResponseHandler<GetPermissionResponse>> AddPermissionAsync(AddUpdatePermissionRequest request)
        {
            ResponseHandler<GetPermissionResponse> response = new();
            var permission = _mapper.Map<Permission>(request);
            permission.Value = PermissionConstants.Permission + "." + permission.Module + "." + permission.Name;
            permission.Slug = $"{PermissionConstants.Permission}_{permission.Module!}_{permission.Name!}".ToLower();
            permission.Type = PermissionConstants.Permission;

            var exists = await _permissionRepository.IsAnyAsync(p => p.Module!.ToLower() == permission.Module!.ToLower() && p.Name!.ToLower() == permission.Name!.ToLower());

            if (exists)
            {
                throw new ApplicationException("Permission already exists");
            }

            var newPermission = await _permissionRepository.AddAsync(permission);

            response.Success = true;
            response.Message = "Permission added successfully";
            response.Data = _mapper.Map<GetPermissionResponse>(newPermission);

            return response;
            
        }
        public async Task<ResponseHandler<string>> DeletePermissionAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var deleted = await _permissionRepository.DeleteAsync(p => p.Id == id);

            response.Success = true;
            response.Message = "Permission deleted successfully";

            return response;
        }

        public async Task<PagedResponseHandler<List<GetPermissionResponse>>> GetAllPermissionAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _permissionRepository.GetAllPaginatedAsync(filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetPermissionResponse>(sn))).ToList();

            PagedResponseHandler<List<GetPermissionResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All permissions retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<GetPermissionResponse>> GetPermissionAsync(Guid permissionId)
        {
            ResponseHandler<GetPermissionResponse> response = new();

            var permission = await _permissionRepository.GetSingleOrDefaultAsync(permissionId);

            response.Success = true;
            response.Message = "Permission retrieved successfully";
            response.Data = _mapper.Map<GetPermissionResponse>(permission);

            return response;
        }

        public async Task<ResponseHandler<GetPermissionResponse>> UpdatePermissionAsync(Guid id, AddUpdatePermissionRequest request)
        {
            ResponseHandler<GetPermissionResponse> response = new();

            var permission = _mapper.Map<Permission>(request);
            permission.Value = PermissionConstants.Permission + "." + permission.Module + "." + permission.Name;
            permission.Slug = $"{PermissionConstants.Permission}_{permission.Module!}_{permission.Name!}".ToLower();

            var exists = await _permissionRepository.IsAnyAsync(p => p.Module!.ToLower() == permission.Module!.ToLower() && p.Name!.ToLower() == permission.Name!.ToLower());
            if (exists)
            {
                throw new ApplicationException("Permission already exists");
            }

            var updatedPermission = await _permissionRepository.UpdateAsync(id, permission);

            response.Success = true;
            response.Message = "Permission updated successfully";
            response.Data = _mapper.Map<GetPermissionResponse>(updatedPermission);

            return response;
        }
    }
}

using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Org.BouncyCastle.Crypto;
using System.Diagnostics.CodeAnalysis;

namespace _750HrsTracker.Services.Implementations
{
    public class PropertyService : BaseService, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper, SessionProvider sessionProvider, IUriService uriService) : base(sessionProvider)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _uriService = uriService;

        }

        public async Task<ResponseHandler<GetPropertyResponse>> AddPropertyAsync(AddUpdatePropertyRequest request)
        {
            ResponseHandler<GetPropertyResponse> response = new();

            var requestData = _mapper.Map<AvailableProperty>(request);
            requestData.TeamId = (Guid)Session.TeamId!;
            requestData.CreatedById = (Guid)Session.UserId!;
            var property = await _propertyRepository.AddAsync(requestData);

            response.Success = true;
            response.Message = "Property added successfully";
            response.Data = _mapper.Map<GetPropertyResponse>(property);

            return response;
        }

        public async Task<ResponseHandler<string>> AssignPropertyToUsersAsync(Guid id, AssignPropertyToUsersRequest request)
        {
            ResponseHandler<string> response = new();

            var property = await _propertyRepository.AssignPropertyToUsersAsync(id, (Guid)Session.TeamId!, request.UserIds!);

            response.Success = true;
            response.Message = "Property assignment completed successfully";

            return response;
        }

        public async Task<ResponseHandler<string>> DeletePropertyAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var property = await _propertyRepository.DeleteAsync(p => p.Id == id && p.TeamId == Session.TeamId);

            response.Success = true;
            response.Message = "Property deleted successfully";

            return response;
        }

        public async Task<PagedResponseHandler<List<GetPropertyResponse>>> GetAllPropertiesAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _propertyRepository.GetAllPaginatedAsync(p => p.TeamId == Session.TeamId!, filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetPropertyResponse>(sn))).ToList();

            PagedResponseHandler<List<GetPropertyResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All properties retrieved successfully";
            return response;
        }
        
        
        public async Task<ResponseHandler<List<GetPropertyResponse>>> GetAllPropertiesAsync()
        {
           
            var properties = await _propertyRepository.GetAllAsync(p => p.TeamId == Session.TeamId!);

            ResponseHandler<List<GetPropertyResponse>> response = new()
            {
                Success = true,
                Message = "All properties retrieved successfully",
                Data = properties!.Select(sn => _mapper.Map<GetPropertyResponse>(sn)).ToList()
            };
            return response;
        }

        public async Task<ResponseHandler<GetPropertyResponse>> GetPropertyAsync(Guid id)
        {
            ResponseHandler<GetPropertyResponse> response = new();

            var property = await _propertyRepository.GetSingleOrDefaultAsync(p => p.Id == id && p.TeamId == Session.TeamId);    

            if (property == null)
            {
                throw new KeyNotFoundException("Property not found");
            };

            response.Success = true;
            response.Message = "Property retrieved successfully";
            response.Data = _mapper.Map<GetPropertyResponse>(property);

            return response;

        }

        public async Task<ResponseHandler<List<GetPropertyResponse>>> SearchPropertiesAsync([NotNull] string keyword)
        {

            ResponseHandler<List<GetPropertyResponse>> response = new();

            var property = await _propertyRepository.SearchEntityAsync(p => p.TeamId == Session.TeamId && p.Name!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Properties retrieved successfully";
            response.Data = property.Select(p => _mapper.Map<GetPropertyResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<GetPropertyResponse>> UpdatePropertyAsync(Guid id, AddUpdatePropertyRequest request)
        {
            ResponseHandler<GetPropertyResponse> response = new();            

            var updatedProperty = await _propertyRepository.UpdateAsync(id, (Guid) Session.TeamId!, _mapper.Map<AvailableProperty>(request));

            response.Success = true;
            response.Message = "Property updated successfully";
            response.Data = _mapper.Map<GetPropertyResponse>(updatedProperty);

            return response;
        }


    }
}

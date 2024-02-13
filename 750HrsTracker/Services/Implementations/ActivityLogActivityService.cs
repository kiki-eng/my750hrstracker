using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;

namespace _750HrsTracker.Services.Implementations
{
    public class ActivityLogActivityService : IActivityLogActivityService
    {
        private readonly IActivityLogActivityRepository _logActivityRepository;
        private readonly IActivityLogCategoryRepository _logCategoryRepository;
        public  readonly IActivityLogSubCategoryRepository _logSubCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ActivityLogActivityService(IActivityLogActivityRepository logActivityRepository, IMapper mapper, IUriService uriService, 
            IActivityLogCategoryRepository logCategoryRepository, IActivityLogSubCategoryRepository logSubCategoryRepository)
        {
            _logActivityRepository = logActivityRepository;
            _logCategoryRepository = logCategoryRepository;
            _logSubCategoryRepository = logSubCategoryRepository;   
            _mapper = mapper;
            _uriService = uriService;

        }

        public async Task<ResponseHandler<GetActivityLogActivityResponse>> AddActivityLogActivityAsync(AddUpdateActivityLogActivityRequest request)
        {
            ResponseHandler<GetActivityLogActivityResponse> response = new();

            var requestData = _mapper.Map<ActivityLogActivity>(request);
            requestData.Slug = Utility.GenerateSlug(request.Name!);

            if (request.PropertyType.Equals(AvailablePropertyType.LTR) )
            {
                if (request.ActivityLogCategoryId is null || request.ActivityLogCategoryId.Equals(Guid.Empty))
                    throw new ApplicationException("LTR activities must belong to a log category");
                var logCategory = await _logCategoryRepository.GetSingleOrDefaultAsync((Guid)request.ActivityLogCategoryId) ?? throw new KeyNotFoundException("Log category not found");
                requestData.ActivityLogCategoryId = logCategory.Id;
            }
            
            var logActivity = await _logActivityRepository.AddAsync(requestData);

            response.Success = true;
            response.Message = "Log activity added successfully";
            response.Data = _mapper.Map<GetActivityLogActivityResponse>(logActivity);

            return response;
        }

        public async Task<ResponseHandler<string>> DeleteActivityLogActivityAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var property = await _logActivityRepository.DeleteAsync(p => p.Id == id);

            response.Success = true;
            response.Message = "Log activity deleted successfully";

            return response;
        }

        public async Task<ResponseHandler<GetActivityLogActivityResponse>> GetActivityLogActivityAsync(Guid id)
        {
            ResponseHandler<GetActivityLogActivityResponse> response = new();

            var logActivity = await _logActivityRepository.GetSingleOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Log activity not found"); 

            response.Success = true;
            response.Message = "Log activity retrieved successfully";
            response.Data = _mapper.Map<GetActivityLogActivityResponse>(logActivity);

            return response;
        }

        public async Task<PagedResponseHandler<List<GetActivityLogActivityResponse>>> GetAllActivityLogActivityAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _logActivityRepository.GetAllPaginatedAsync(filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetActivityLogActivityResponse>(sn))).ToList();

            PagedResponseHandler<List<GetActivityLogActivityResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All log activities retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<List<GetActivityLogActivityResponse>>> SearchActivityLogActivityAsync(string keyword)
        {
            ResponseHandler<List<GetActivityLogActivityResponse>> response = new();

            var property = await _logActivityRepository.SearchEntityAsync(p => p.Name!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Log activities retrieved successfully";
            response.Data = property.Select(p => _mapper.Map<GetActivityLogActivityResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<GetActivityLogActivityResponse>> UpdateActivityLogActivityAsync(Guid id, AddUpdateActivityLogActivityRequest request)
        {
            ResponseHandler<GetActivityLogActivityResponse> response = new();

            var requestData = _mapper.Map<ActivityLogActivity>(request);
            requestData.Slug = Utility.GenerateSlug(request.Name!);

            if (request.PropertyType.Equals(AvailablePropertyType.LTR))
            {
                if (request.ActivityLogCategoryId is null || request.ActivityLogCategoryId.Equals(Guid.Empty))
                    throw new ApplicationException("LTR activities must belong to a log category");
                var logCategory = await _logCategoryRepository.GetSingleOrDefaultAsync((Guid)request.ActivityLogCategoryId) ?? throw new KeyNotFoundException("Log category not found");

                requestData.ActivityLogCategoryId = logCategory.Id;
            }

            var updatedLogActivity = await _logActivityRepository.UpdateAsync(id, requestData);

            response.Success = true;
            response.Message = "Log activity updated successfully";
            response.Data = _mapper.Map<GetActivityLogActivityResponse>(updatedLogActivity);

            return response;
        }


        #region Log Sub Category APIs

        public async Task<ResponseHandler<GetLogActivitySubCategoryResponse>> AddLogActivitySubCategoryAsync(Guid logActivityId, AddUpdateLogActivitySubCategoryRequest request)
        {
            ResponseHandler<GetLogActivitySubCategoryResponse> response = new();

            var logActivity = await _logActivityRepository.GetSingleOrDefaultAsync(logActivityId) ?? throw new KeyNotFoundException("Log activity not found");

            var requestData = _mapper.Map<ActivityLogSubCategory>(request);
            requestData.Slug = Utility.GenerateSlug(request.Name!);
            requestData.LogActivityId = logActivity.Id;
            var property = await _logSubCategoryRepository.AddAsync(requestData);

            response.Success = true;
            response.Message = "Log activity sub category added successfully";
            response.Data = _mapper.Map<GetLogActivitySubCategoryResponse>(property);

            return response;
        }

        public async Task<ResponseHandler<GetLogActivitySubCategoryResponse>> GetLogActivitySubCategoryAsync(Guid id)
        {
            ResponseHandler<GetLogActivitySubCategoryResponse> response = new();

            var logActivitySubCategory = await _logSubCategoryRepository.GetSingleOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Log activity sub category not found");

            response.Success = true;
            response.Message = "Log activity sub category retrieved successfully";
            response.Data = _mapper.Map<GetLogActivitySubCategoryResponse>(logActivitySubCategory);

            return response;
        }

        public async Task<PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>> GetAllLogActivitySubCategoryAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var logSubCategories = await _logSubCategoryRepository.GetAllPaginatedAsync(filter);


            var pagedData = (logSubCategories.Records!.Select(sn => _mapper.Map<GetLogActivitySubCategoryResponse>(sn))).ToList();

            PagedResponseHandler<List<GetLogActivitySubCategoryResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, logSubCategories.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All log activities sub categories retrieved successfully";
            return response;
        }

        public async Task<PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>> GetAllLogActivitySubCategoryAsync(Guid logActivityId, PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var logSubCategories = await _logSubCategoryRepository.GetAllPaginatedAsync(sc => sc.LogActivityId == logActivityId, filter);


            var pagedData = (logSubCategories.Records!.Select(sn => _mapper.Map<GetLogActivitySubCategoryResponse>(sn))).ToList();

            PagedResponseHandler<List<GetLogActivitySubCategoryResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, logSubCategories.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All log activities sub categories retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<List<GetLogActivitySubCategoryResponse>>> SearchLogActivitySubCategoryAsync(string keyword)
        {
            ResponseHandler<List<GetLogActivitySubCategoryResponse>> response = new();

            var property = await _logSubCategoryRepository.SearchEntityAsync(p => p.Name!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Log activity sub categories retrieved successfully";
            response.Data = property.Select(p => _mapper.Map<GetLogActivitySubCategoryResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<GetLogActivitySubCategoryResponse>> UpdateLogActivitySubCategoryAsync(Guid id, AddUpdateLogActivitySubCategoryRequest request)
        {
            ResponseHandler<GetLogActivitySubCategoryResponse> response = new();


            var requestData = _mapper.Map<ActivityLogSubCategory>(request);
            requestData.Slug = Utility.GenerateSlug(request.Name!);
            var property = await _logSubCategoryRepository.UpdateAsync(id, requestData);

            response.Success = true;
            response.Message = "Log activity sub category added successfully";
            response.Data = _mapper.Map<GetLogActivitySubCategoryResponse>(property);

            return response;
        }

        public async Task<ResponseHandler<string>> DeleteLogActivitySubCategoryAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var property = await _logSubCategoryRepository.DeleteAsync(p => p.Id == id);

            response.Success = true;
            response.Message = "Log activity sub category deleted successfully";

            return response;
        }

        #endregion
    }
}

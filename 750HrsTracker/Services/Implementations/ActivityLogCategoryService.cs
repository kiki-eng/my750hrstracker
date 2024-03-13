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
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Services.Implementations
{
    public class ActivityLogCategoryService : IActivityLogCategoryService
    {
        private readonly IActivityLogCategoryRepository _logCategoryRepository;
        private readonly IActivityLogActivityRepository _logActivityRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ActivityLogCategoryService(IActivityLogCategoryRepository logCategoryRepository, IMapper mapper, IUriService uriService, IActivityLogActivityRepository logActivityRepository)
        {
            _logCategoryRepository = logCategoryRepository;
            _logActivityRepository = logActivityRepository;
            _mapper = mapper;
            _uriService = uriService;

        }

        public async Task<ResponseHandler<GetActivityLogCategoryResponse>> AddActivityLogCategoryAsync(AddActivityLogCategoryRequest request)
        {
            ResponseHandler<GetActivityLogCategoryResponse> response = new();

            if (!request.PropertyType.Equals(AvailablePropertyType.LTR))
            {
                throw new ApplicationException("Categories can only be created for LTR property types");
            }

            ActivityLogCategory requestData = new()
            {
                Name = request.Name,
                Slug = Utility.GenerateSlug(request.Name!),
            };

            List<ActivityLogActivity> logActivities = new();
            foreach (var logActivity in request.LogActivities!)
            {
                ActivityLogActivity activityLogActivity = new()
                {
                    Name = logActivity.Name,
                    Slug = Utility.GenerateSlug(logActivity.Name!),
                    AvailablePropertyType = AvailablePropertyType.LTR,
                };

                logActivities.Add(activityLogActivity);
            }

            requestData.ActivityLogActivities = logActivities;


            var property = await _logCategoryRepository.AddAsync(requestData);

            response.Success = true;
            response.Message = "Log category added successfully";
            response.Data = _mapper.Map<GetActivityLogCategoryResponse>(property);

            return response;
        }

        public async Task<ResponseHandler<string>> DeleteActivityLogCategoryAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var property = await _logCategoryRepository.DeleteAsync(p => p.Id == id);

            response.Success = true;
            response.Message = "Log category deleted successfully";

            return response;
        }

        public async Task<ResponseHandler<GetActivityLogCategoryResponse>> GetActivityLogCategoryAsync(Guid id)
        {
            ResponseHandler<GetActivityLogCategoryResponse> response = new();

            var logCategory = await _logCategoryRepository.GetSingleOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Log activity not found"); 

            response.Success = true;
            response.Message = "Log category retrieved successfully";
            response.Data = _mapper.Map<GetActivityLogCategoryResponse>(logCategory);

            return response;
        }

        public async Task<PagedResponseHandler<List<GetActivityLogCategoryResponse>>> GetAllActivityLogCategoryAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _logCategoryRepository.GetAllPaginatedAsync(filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetActivityLogCategoryResponse>(sn))).ToList();

            PagedResponseHandler<List<GetActivityLogCategoryResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All log categories retrieved successfully";
            return response;
        }
        
        public async Task<ResponseHandler<List<GetLogCategoryResponse>>> GetAllActivityLogCategoryAsync()
        {
          
            var logCategories = await _logCategoryRepository.GetLogCategories();

            var responseData = (logCategories.Select(sn => MappedResponse(sn))).ToList();

            ResponseHandler<List<GetLogCategoryResponse>> response = new()
            {
                Success = true,
                Message = "All log categories retrieved successfully",
                Data = responseData
            };
            return response;
        }

        public async Task<ResponseHandler<List<GetActivityLogCategoryResponse>>> SearchActivityLogCategoryAsync(string keyword)
        {
            ResponseHandler<List<GetActivityLogCategoryResponse>> response = new();

            var property = await _logCategoryRepository.SearchEntityAsync(p => p.Name!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Log categories retrieved successfully";
            response.Data = property.Select(p => _mapper.Map<GetActivityLogCategoryResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<GetActivityLogCategoryResponse>> UpdateActivityLogCategoryAsync(Guid id, UpdateActivityLogCategoryRequest request)
        {
            ResponseHandler<GetActivityLogCategoryResponse> response = new();

            var updatedLogCategory = await _logCategoryRepository.UpdateAsync(id, _mapper.Map<ActivityLogCategory>(request));

            response.Success = true;
            response.Message = "Log category updated successfully";
            response.Data = _mapper.Map<GetActivityLogCategoryResponse>(updatedLogCategory);

            return response;
        }

        private GetLogCategoryResponse MappedResponse(ActivityLogCategory logCategory)
        {
            var response = new GetLogCategoryResponse
            {
                Id = logCategory.Id,
                Name = logCategory.Name,
                PropertyType = (AvailablePropertyType)logCategory.AvailablePropertyType!,
                CreatedAt = logCategory.CreatedAt
            };

            if (logCategory.ActivityLogActivities != null || logCategory.ActivityLogActivities!.Count > 0)
            {
                List<GetActivityLogActivityResponse> getActivityLogActivityResponses = new List<GetActivityLogActivityResponse>();

                foreach(var activity in  logCategory.ActivityLogActivities)
                {
                    GetActivityLogActivityResponse logActivityResponse = new GetActivityLogActivityResponse()
                    {
                        Name = activity.Name,
                        Id = activity.Id,
                        Slug = activity.Slug,
                        PropertyType =(AvailablePropertyType)activity.AvailablePropertyType!,
                        CreatedAt = activity.CreatedAt
                            
                    };

                    if (activity.ActivityLogSubCategories != null || activity.ActivityLogSubCategories!.Count > 0)
                    {
                        List<GetLogActivitySubCategoryResponse> tasks = new List<GetLogActivitySubCategoryResponse>();

                        foreach (var sc in activity.ActivityLogSubCategories)
                        {
                            GetLogActivitySubCategoryResponse subCategory = new GetLogActivitySubCategoryResponse()
                            {
                                Name = sc.Name,
                                Id = sc.Id,
                                Slug = activity.Slug,
                            };


                            tasks.Add(subCategory);
                        }

                        logActivityResponse.Tasks = tasks;
                    }

                    getActivityLogActivityResponses.Add(logActivityResponse);
                }

                response.Activities = getActivityLogActivityResponses;
            }

            return response;
        }
    }
}

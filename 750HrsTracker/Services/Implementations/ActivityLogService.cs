using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using Microsoft.Extensions.Options;
using _750HrsTracker.Enums;

namespace _750HrsTracker.Services.Implementations
{
    public class ActivityLogService : BaseService, IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly AppSettings _appSettings;

        public ActivityLogService(IActivityLogRepository activityLogRepository, IMapper mapper, SessionProvider sessionProvider, 
            IUriService uriService, IUserRepository userRepository, IOptionsSnapshot<AppSettings> appSettings) : base(sessionProvider)
        {
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
            _uriService = uriService;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<ResponseHandler<GetActivityLogResponse>> AddActivityLogAsync(AddActivityLogRequest request)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            // validate supporting document
            var file = request.SupportingDocument!;
            if (file.Length < 1)
            {
                throw new ApplicationException("invalid file submitted");
            }

            var requestData = _mapper.Map<ActivityLog>(request);
            requestData.TeamId = (Guid)Session.TeamId!;
            requestData.CreatedById = (Guid)Session.UserId!;

            var activityBy = await _userRepository.GetUserAsync(request.ActivityById);

            requestData.ActivityById = activityBy.Id;

            var activityLog = await _activityLogRepository.AddAsync(requestData);

            ActivityLogDocument? documentUploadResponse = await Storage.PrepareAndUploadDocumentAsync(file, _appSettings, (Guid)Session.TeamId, DocumentFor.ActivityLog);

            if(documentUploadResponse != null)
            {
                await _activityLogRepository.AttachLogDocumentAsync(documentUploadResponse!);

            }

            List<ActivityLogProperty> properties = new(); 
            foreach(var propertyId in request.PropertiesIds!)
            {
                ActivityLogProperty activityLogProperty = new() 
                { 
                    ActivityLogId = activityLog.Id,
                    PropertyId = propertyId,
                };
                properties.Add(activityLogProperty);
            }
            await _activityLogRepository.AttachLogPropertyAsync(properties);


            response.Success = true;
            response.Message = "Activity log added successfully";
            response.Data = _mapper.Map<GetActivityLogResponse>(activityLog);

            return response;
        }


        public async Task<ResponseHandler<string>> DeleteActivityLogAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var activityLog = await _activityLogRepository.DeleteAsync(p => p.Id == id && p.TeamId == Session.TeamId);

            response.Success = true;
            response.Message = "Activity log deleted successfully";

            return response;
        }

        public async Task<PagedResponseHandler<List<GetActivityLogResponse>>> GetAllActivityLogAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _activityLogRepository.GetAllPaginatedAsync(p => p.TeamId == Session.TeamId!, filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetActivityLogResponse>(sn))).ToList();

            PagedResponseHandler<List<GetActivityLogResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All activity logs retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<GetActivityLogResponse>> GetActivityLogAsync(Guid id)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            var ActivityLog = await _activityLogRepository.GetSingleOrDefaultAsync(p => p.Id == id && p.TeamId == Session.TeamId);

            if (ActivityLog == null)
            {
                throw new KeyNotFoundException("Activity log not found");
            };

            response.Success = true;
            response.Message = "Activity log retrieved successfully";
            response.Data = _mapper.Map<GetActivityLogResponse>(ActivityLog);

            return response;

        }

        public async Task<ResponseHandler<List<GetActivityLogResponse>>> SearchActivityLogAsync([NotNull] string keyword)
        {

            ResponseHandler<List<GetActivityLogResponse>> response = new();

            var ActivityLog = await _activityLogRepository.SearchEntityAsync(p => p.TeamId == Session.TeamId && p.Name!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Activity logs retrieved successfully";
            response.Data = ActivityLog.Select(p => _mapper.Map<GetActivityLogResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<GetActivityLogResponse>> UpdateActivityLogAsync(Guid id, UpdateActivityLogRequest request)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            var updatedActivityLog = await _activityLogRepository.UpdateAsync(id, (Guid)Session.TeamId!, _mapper.Map<ActivityLog>(request));

            response.Success = true;
            response.Message = "Activity log updated successfully";
            response.Data = _mapper.Map<GetActivityLogResponse>(updatedActivityLog);

            return response;
        }

        public async Task<ResponseHandler<GetDashboardResponse>> GetDashboardDataAsync(AvailablePropertyType availablePropertyType)
        {
            ResponseHandler<GetDashboardResponse> response = new();
            GetDashboardResponse responseData = await _activityLogRepository.GetRecentActivityLogsAsync((Guid) Session.TeamId!, availablePropertyType);

            response.Success = true;
            response.Message = "Dashboard data retrieved successfully";
            response.Data = responseData;
            return response;


        }
    }
}

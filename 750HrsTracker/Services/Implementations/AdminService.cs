using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace _750HrsTracker.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private IMapper _mapper;
        private ITeamRepository _teamRepository;
        private IPropertyRepository _propertyRepository;
        private IUriService _uriService;
        private INotificationService _notificationService;
        private ISubscriptionRepository _subscriptionRepository;
        private readonly IActivityLogRepository _activityLogRepository; 
        private AppSettings _appSettings;
        public AdminService(IMapper mapper, ITeamRepository teamRepository, IUriService uriService, 
            INotificationService notificationService, ISubscriptionRepository subscriptionRepository, IOptionsSnapshot<AppSettings> appSettings, 
            IActivityLogRepository activityLogRepository, IPropertyRepository propertyRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _uriService = uriService;
            _notificationService = notificationService;
            _subscriptionRepository = subscriptionRepository;
            _appSettings = appSettings.Value;
            _activityLogRepository = activityLogRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<PagedResponseHandler<List<GetTeamResponse>>> GetAllTeamsAsync(PaginationFilter filter, HttpRequest httpRequest)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var teams = await _teamRepository.GetAllTeamsAsync(validFilters);

            var pagedData = (teams.Records!.Select(sn => new GetTeamResponse
            {
                Id = sn.Id,
                Name = sn.Name,
                CreatedBy = $"{sn.Owner!.Firstname} {sn.Owner!.Lastname} ({sn.Owner!.Email})",
                CreatedAt = sn.CreatedAt
            })).ToList();


            PagedResponseHandler<List<GetTeamResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, teams.TotalCount, _uriService, httpRequest.Path);

            response.Success = true;
            response.Message = "All teams retrieved successfully";
            return response;

        }

        public async Task<ResponseHandler<AdminGetTeamResponse>> GetTeamAsync(Guid teamId)
        {
            ResponseHandler<AdminGetTeamResponse> response = new();

            var team = await _teamRepository.GetTeamAsync(teamId);

            var properties = await _propertyRepository.GetAllAsync(p => p.TeamId == teamId);
            var activityLogs = await _activityLogRepository.GetAllAsync(a => a.TeamId == teamId); 
            
            var groupedLogs = activityLogs.ToList().GroupBy(a => a.PropertyType);

            decimal totalLogHours = 0;
            decimal totalStrHours = 0;
            decimal totalLtrHours = 0;
            foreach ( var group in groupedLogs)
            {

                var totalGroupedHours = group.ToList().Sum(l => l.HoursSpent);
                var totalGroupedMinutes = group.ToList().Sum(l => l.MinutesSpent);
                var totalGroupedSeconds = group.ToList().Sum(l => l.HoursSpent);
                var totalGroupedTimeInSeconds = (totalGroupedHours * 3600) + (totalGroupedMinutes * 60) + totalGroupedSeconds;

                decimal totalGroupedRepHours = totalGroupedTimeInSeconds / 3600;
                totalLogHours += Math.Round(totalGroupedRepHours, 2);

                if(group.Key == AvailablePropertyType.LTR)
                {
                    totalLtrHours = totalGroupedRepHours;
                }
                else if(group.Key == AvailablePropertyType.STR)
                {
                    totalStrHours = totalGroupedRepHours;
                }
            }

            var responseData = new AdminGetTeamResponse
            {
                Id = team.Id,
                CreatedAt = team.CreatedAt,
                CreatedBy = $"{team.Owner!.Firstname} {team.Owner!.Lastname} ({team.Owner!.Email})",
                ModifiedAt = team.ModifiedAt,
                Name = team.Name,
                Properties = properties.Take(5).Select(p => new GetPropertyResponse { Id = p.Id, Address = p.Address, Code = p.Code, Name = p.Name }).ToList(),
                Users = team.TeamUsers!.Take(5).Select(u => new GetUsersOnlyResponse
                {
                    FirstName = u.User!.Firstname,
                    LastName = u.User!.Lastname,
                    Id  = u.User!.Id,
                    Email = u.User!.Email,
                }).ToList(),
                TotalNumberOfLogs = activityLogs.Count(), 
                TotalHours = totalLogHours,
                TotalLtrHours = totalLtrHours,
                TotalStrHours = totalStrHours,
            };


            response.Success = true;
            response.Message = "Team details retrieved successfully";
            response.Data = responseData;

            return response;
        }

        public async Task<ResponseHandler<List<GetSubscriptionResponse>>> GetSubscriptionsAsync()
        {
            ResponseHandler<List<GetSubscriptionResponse>> response = new();

            var subscriptions = await _subscriptionRepository.GetAllAsync();

            response.Success = true;
            response.Message = "Subscriptions retrieved successfully";
            response.Data = subscriptions.Select(s => _mapper.Map<GetSubscriptionResponse>(s)).ToList();    

            return response;            
        }

        public async Task<ResponseHandler<string>> SendSupportNotificationAsync(SupportRequest supportRequest)
        {
            ResponseHandler<string> response = new();

            var notificationSent = await _notificationService.SendSupportNotificationAsync(supportRequest);

            response.Success = notificationSent;
            response.Message = notificationSent ? "Your support request has been sent successfully" : "Could not send support request. Please try again";

            return response;    
        }

        public async Task<ResponseHandler<GetAutoSuggestionResponse>> GetAutoSuggestionResponseAsync (string keyword)
        {
            ResponseHandler<GetAutoSuggestionResponse> response = new();
            var requestUri = $"{_appSettings.AutoSuggestionRequestUrl}?input={keyword}&key={_appSettings.AutoSuggestionApiKey}";
            var httpResponse = await Utility.MakeHttpRequest(null!, _appSettings.AutoSuggestionBaseUrl!, requestUri, HttpMethod.Get);

            if (httpResponse != null && httpResponse.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<GetAutoSuggestionResponse>(await httpResponse.Content.ReadAsStringAsync()!);

                response.Success = true;
                response.Message = "Suggestions retrieved successfully";
                response.Data = responseData;   
            }
            else
            {
                throw new ApplicationException("Unable to fetch suggestions");
            }

            return response;
        }

        public async Task<ResponseHandler<AdminGetActivityLogResponse>> GetActivityLogAsync(Guid id)
        {
            ResponseHandler<AdminGetActivityLogResponse> response = new();

            var activityLog = await _activityLogRepository.GetLogByIdAsync(id) ?? throw new KeyNotFoundException("Activity log not found");

            
            var responseData = MappedActivityLogResponse(activityLog);


            var documents = await _activityLogRepository.GetDocumentsAsync(activityLog.Id);
            List<Base64FileModel> supportDocuments = new();

            if (documents != null && documents.Count > 0)
            {
                foreach (var document in documents)
                {
                    byte[] fileBytes = await Storage.DownloadDocumentAsStream(_appSettings, document.RemoteDirectoryName!);

                    Base64FileModel fileModel = new()
                    {
                        ContentType = Utility.GetMimeType(document.DocumentName!),
                        FileExtension = Path.GetExtension(document.DocumentName!),
                        Data = Convert.ToBase64String(fileBytes),
                        FileName = document.DocumentName,
                        DocumentId = document.Id
                    };

                    supportDocuments.Add(fileModel);
                }
            }

            responseData.SupportingDocuments = supportDocuments;

            response.Success = true;
            response.Message = "Activity log retrieved successfully";
            response.Data = responseData;

            return response;
        }

        public async Task<PagedResponseHandler<List<AdminGetActivityLogResponse>>> GetAllActivityLogAsync(AvailablePropertyType propertyType, PaginationFilter filter, ActivityLogFilter activityLogFilter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var validActivityLogFilters = new ActivityLogFilter(activityLogFilter.Activity.ToString(), activityLogFilter.Property.ToString(), activityLogFilter.Member.ToString(),
                activityLogFilter.AllSupportingDocument, activityLogFilter.HasSupportingDocument, activityLogFilter.WithDocuments, activityLogFilter.StartDate, activityLogFilter.EndDate);

            var logs = await _activityLogRepository.GetAllLogsAsync(validFilters, validActivityLogFilters, propertyType);


            var pagedData = (logs.Records!.Select(sn => MappedActivityLogResponse(sn))).ToList();

            PagedResponseHandler<List<AdminGetActivityLogResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, logs.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All activity logs retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<List<AdminGetActivityLogResponse>>> SearchActivityLogAsync(string keyword)
        {
            ResponseHandler<List<AdminGetActivityLogResponse>> response = new();

            var activityLog = await _activityLogRepository.SearchEntityAsync(p => p.Description!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Activity logs retrieved successfully";
            response.Data = activityLog.Select(p => _mapper.Map<AdminGetActivityLogResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<AdminGetPropertyResponse>> GetPropertyAsync(Guid propertyId)
        {
            ResponseHandler<AdminGetPropertyResponse> response = new();

            var property = await _propertyRepository.GetSingleOrDefaultAsync(propertyId) ?? throw new KeyNotFoundException("Property not found");

            response.Success = true;
            response.Message = "Retrieved property details successfully";
            response.Data = _mapper.Map<AdminGetPropertyResponse>(property);

            return response;
        }

        public async Task<PagedResponseHandler<List<AdminGetPropertyResponse>>> GetAllPropertiesAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var properties = await _propertyRepository.GetAllPaginatedAsync(validFilters, orderByDescending: e => e.CreatedAt, e => e.Team!);


            var pagedData = (properties.Records!.Select(sn => MapProperty(sn))).ToList();

            PagedResponseHandler<List<AdminGetPropertyResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All properties retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<List<AdminGetPropertyResponse>>> SearchPropertiesAsync(string keyword)
        {
            ResponseHandler<List<AdminGetPropertyResponse>> response = new();

            var properties = await _propertyRepository.SearchEntityAsync(p => p.Name!.ToLower().Contains(keyword.ToLower()));

            response.Success = true;
            response.Message = "Properties retrieved successfully";
            response.Data = properties.Select(p => _mapper.Map<AdminGetPropertyResponse>(p)).ToList();

            return response;
        }


        #region Data Maps
        private AdminGetActivityLogResponse MappedActivityLogResponse(ActivityLog activityLog)
        {
            var response = _mapper.Map<AdminGetActivityLogResponse>(activityLog);


            if(activityLog.Team is not null)
            {
                response.Team = activityLog.Team?.Name + "'s team";
            }
            if (activityLog.ActivityLogActivity != null)
            {
                response.Activity = new GetActivityLogActivityResponse
                {
                    Name = activityLog.ActivityLogActivity.Name,
                    Id = activityLog.ActivityLogActivity.Id
                };

                //response.Activity = activityLog.ActivityLogActivity.Name;
            }

            if (activityLog.Task != null)
            {
                response.Task = new GetLogActivitySubCategoryResponse()
                {
                    Id = activityLog.Task.Id,
                    Name = activityLog.Task.Name,
                    Slug = activityLog.Task.Slug,
                };
            }

            if (activityLog.ActivityLogActivity != null && activityLog.ActivityLogActivity.ActivityLogCategory != null)
            {
                response.Category = new GetActivityLogCategoryResponse
                {
                    Name = activityLog.ActivityLogActivity.ActivityLogCategory.Name,
                    Id = activityLog.ActivityLogActivity.ActivityLogCategory.Id,
                };

                //response.Category = activityLog.ActivityLogActivity.ActivityLogCategory.Name;
            }

            if (activityLog.ActivityLogProperties != null && activityLog.ActivityLogProperties.Count > 0)
            {
                List<GetPropertyResponse> properties = new List<GetPropertyResponse>();

                foreach (var property in activityLog.ActivityLogProperties)
                {
                    GetPropertyResponse propertyResponse = new()
                    {
                        Name = property.Property?.Name,
                        Description = property.Property?.Description,
                        Id = property.PropertyId,
                    };

                    properties.Add(propertyResponse);
                }

                response.Properties = properties;
            }

            return response;
        }


        private AdminGetPropertyResponse MapProperty(AvailableProperty property)
        {
            var response = _mapper.Map<AdminGetPropertyResponse>(property);

            if(property.Team != null)
            {
                response.Team = $"{property.Team!.Name}";
            }

            return response;
        }
        #endregion
    }
}

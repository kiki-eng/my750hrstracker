using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace _750HrsTracker.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private IMapper _mapper;
        private ITeamRepository _teamRepository;
        private IUriService _uriService;
        private INotificationService _notificationService;
        private ISubscriptionRepository _subscriptionRepository;
        private AppSettings _appSettings;
        public AdminService(IMapper mapper, ITeamRepository teamRepository, IUriService uriService, 
            INotificationService notificationService, ISubscriptionRepository subscriptionRepository, IOptionsSnapshot<AppSettings> appSettings)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _uriService = uriService;
            _notificationService = notificationService;
            _subscriptionRepository = subscriptionRepository;
            _appSettings = appSettings.Value;
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
    }
}

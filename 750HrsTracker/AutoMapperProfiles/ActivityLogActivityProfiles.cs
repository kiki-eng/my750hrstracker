using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.ActivityLogModels;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class ActivityLogActivityProfiles : Profile
    {
        public ActivityLogActivityProfiles()
        {
            CreateMap<ActivityLogActivity, GetActivityLogActivityResponse>();
            CreateMap<AddUpdateActivityLogActivityRequest, ActivityLogActivity>();
        }
    }
}

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
            CreateMap<ActivityLogActivity, GetActivityLogActivityResponse>().ForMember(s => s.PropertyType, sp => sp.MapFrom(o => o.AvailablePropertyType));
            CreateMap<AddUpdateActivityLogActivityRequest, ActivityLogActivity>();
            CreateMap<ActivityLogSubCategory, GetLogActivitySubCategoryResponse>();


            CreateMap<AddActivityLogRequest, ActivityLog>();
            CreateMap<ActivityLog, GetActivityLogResponse>();
        }
    }
}

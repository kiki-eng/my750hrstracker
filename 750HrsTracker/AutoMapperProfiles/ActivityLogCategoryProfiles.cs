using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.ActivityLogModels;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class ActivityLogCategoryProfiles : Profile
    {
        public ActivityLogCategoryProfiles()
        {
            CreateMap<ActivityLogCategory, GetActivityLogCategoryResponse>().ForMember(s => s.PropertyType, sp => sp.MapFrom(o => o.AvailablePropertyType));
            CreateMap<UpdateActivityLogCategoryRequest, ActivityLogCategory>();
            CreateMap<AddActivityLogCategoryRequest, ActivityLogCategory>();
        }
    }
}

using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class PropertyProfiles : Profile
    {
        public PropertyProfiles()
        {
            CreateMap<AvailableProperty, GetPropertyResponse>();
            CreateMap<AvailableProperty, AdminGetPropertyResponse>().ForMember(s => s.Type, sp => sp.MapFrom(o => o.PropertyType));
            CreateMap<AddUpdatePropertyRequest, AvailableProperty>();   
        }
    }
}

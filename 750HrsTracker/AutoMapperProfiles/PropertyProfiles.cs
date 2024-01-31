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
            CreateMap<AddUpdatePropertyRequest, AvailableProperty>();   
        }
    }
}

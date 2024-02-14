using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.SubscriptionModels;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class SubscriptionProfiles : Profile
    {
        public SubscriptionProfiles() 
        {
            CreateMap<Subscription, GetSubscriptionResponse>();
            CreateMap<AddUpdateSubscriptionRequest, Subscription>();
            CreateMap<Subscription, UpdateSubscriptionPermissionResponse>()
                .ForMember(s => s.SubscriptionId, sp => sp.MapFrom(o => o.Id))
                .ForMember(s => s.SubscriptionName, sp => sp.MapFrom(o => o.Name));
        }
    }
}

using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.SubscriptionModels;
using AutoMapper;
using Newtonsoft.Json;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class SubscriptionProfiles : Profile
    {
        public SubscriptionProfiles() 
        {
            CreateMap<Subscription, GetSubscriptionResponse>()
                .ForMember(s => s.Features, sp => sp.MapFrom(o => JsonConvert.DeserializeObject<List<string>>(o.Features!)));
            CreateMap<AddUpdateSubscriptionRequest, Subscription>();
            CreateMap<Subscription, UpdateSubscriptionPermissionResponse>()
                .ForMember(s => s.SubscriptionId, sp => sp.MapFrom(o => o.Id))
                .ForMember(s => s.SubscriptionName, sp => sp.MapFrom(o => o.Name));
        }
    }
}

using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class TeamProfiles : Profile
    {
        public TeamProfiles()
        {
            CreateMap<Team, GetTeamResponse>();
        }
    }
}

using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class RoleProfiles : Profile
    {
        public RoleProfiles()
        {
            CreateMap<Role, GetRolesOnlyResponse>();
            CreateMap<Role, GetRoleResponse>();
            CreateMap<AddUpdateRolesRequest, Role>();
        }
    }
}

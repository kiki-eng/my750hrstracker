using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models;
using _750HrsTracker.Models.Admin;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<User, GetUsersOnlyResponse>();
            CreateMap<User, SignInResponse>();
            CreateMap<User, UserUtilData>();
            CreateMap<User, UpdateUserSecurityRequest>();
            
            
            CreateMap<Admin, GetUsersOnlyResponse>();
            CreateMap<Admin, SignInResponse>();
            CreateMap<Admin, UserUtilData>();
            CreateMap<Admin, UpdateUserSecurityRequest>();
        }
    }
}

using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models;
using AutoMapper;

namespace _750HrsTracker.AutoMapperProfiles
{
    public class PermissionProfiles : Profile
    {
        public PermissionProfiles()
        {
            CreateMap<Permission, GetPermissionResponse>();
            CreateMap<AddUpdatePermissionRequest, Permission>();
        }
    }
}

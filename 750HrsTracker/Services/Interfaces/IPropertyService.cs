using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<ResponseHandler<GetPropertyResponse>> AddPropertyAsync(AddUpdatePropertyRequest request); 
        Task<ResponseHandler<GetPropertyResponse>> GetPropertyAsync(Guid id); 
        Task<PagedResponseHandler<List<GetPropertyResponse>>> GetAllPropertiesAsync(PaginationFilter filter, string route); 
        Task<ResponseHandler<List<GetPropertyResponse>>> GetAllPropertiesAsync(); 
        Task<ResponseHandler<List<GetPropertyResponse>>> SearchPropertiesAsync(string keyword); 
        Task<ResponseHandler<GetPropertyResponse>> UpdatePropertyAsync(Guid id, AddUpdatePropertyRequest request); 
        Task<ResponseHandler<string>> DeletePropertyAsync(Guid id); 
        Task<ResponseHandler<string>> AssignPropertyToUsersAsync(Guid id, AssignPropertyToUsersRequest request); 
    }
}

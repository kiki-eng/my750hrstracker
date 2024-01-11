using _750HrsTracker.Filters;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace _750HrsTracker.Services.Implementations
{
    public class UriService : IUriService
    {
        private readonly IHttpContextAccessor _accessor;
        public UriService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var request = _accessor?.HttpContext?.Request;
            var baseUri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());

            Uri endpointUri = new Uri(string.Concat(baseUri, route));
            string modifiedUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUrl = QueryHelpers.AddQueryString(modifiedUrl, "pageSize", filter.PageSize.ToString());

            return new Uri(modifiedUrl);
        }
    }
}

using _750HrsTracker.Filters;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}

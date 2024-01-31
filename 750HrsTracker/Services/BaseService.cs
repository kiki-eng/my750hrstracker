using _750HrsTracker.Models.Misc;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Providers;

namespace _750HrsTracker.Services
{
    public class BaseService
    {
        public AppSession Session;

        public BaseService(SessionProvider sessionProvider)
        {
            Session = sessionProvider.Session;
        }
    }
}

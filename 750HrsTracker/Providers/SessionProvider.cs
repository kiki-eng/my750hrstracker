using _750HrsTracker.Models;
using _750HrsTracker.Models.Misc;

namespace _750HrsTracker.Providers
{
    public class SessionProvider
    {
        public AppSession Session;

        public SessionProvider()
        {
            Session = new();
        }

        public void Initialise(User user)
        {
            Session.User = user;
            Session.UserId = user.Id;
            Session.TeamId = user.DefaultTeamId != null ? Guid.Parse(user.DefaultTeamId!) : Guid.Empty;
        }
    }
}

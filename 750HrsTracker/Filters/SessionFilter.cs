using _750HrsTracker.Models;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _750HrsTracker.Filters
{
    public class SessionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {

            var serviceProvider = context.HttpContext.RequestServices;
            var sessionProvider = serviceProvider.GetService<SessionProvider>();
            var userRepository = serviceProvider.GetService<IUserRepository>();

            var user = await userRepository!.GetUserAsync(Guid.Parse(context.HttpContext.User.Claims.First(x => x.Type == "Id").Value));
            if (user != null)
            {
                sessionProvider!.Initialise(user);
            }
            var resultContext = await next();
            }
    }
}

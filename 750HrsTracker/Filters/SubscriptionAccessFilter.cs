using _750HrsTracker.Helpers;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _750HrsTracker.Filters
{
    public class SubscriptionAccessFilter : IAsyncActionFilter
    {
        private readonly string _permission;
        public SubscriptionAccessFilter(string permission)
        {

            _permission = permission;

        }
        public async Task OnActionExecutionAsync(
           ActionExecutingContext context,
           ActionExecutionDelegate next)
        {

            try
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var userRepository = serviceProvider.GetService<IUserRepository>();
                var teamSubscriptionRepository = serviceProvider.GetService<ITeamSubscriptionRepository>();

                var user = await userRepository!.GetUserAsync(Guid.Parse(context.HttpContext.User.Claims.First(x => x.Type == "Id").Value)) ?? throw new ForbiddenAccessException("Could not validate user");

                var teamSubscription = await teamSubscriptionRepository!.FindPermissionInTeamSubscriptionAsync(Guid.Parse(user.DefaultTeamId!), _permission) ?? throw new ForbiddenAccessException("Cannot access resource. Kindly subscribe to continue");


                var currentDate = DateTime.Now;
                var gracePeriodDate = teamSubscription!.EndDate!.Value.AddMinutes(teamSubscription.GracePeriodMinutes);

                if (currentDate >= teamSubscription.EndDate && currentDate >= gracePeriodDate)
                    throw new ForbiddenAccessException("Subscription expired. Subscribe again to continue");


                var resultContext = await next();
            }catch(Exception ex)
            {
                throw new ForbiddenAccessException(ex.Message);
            }
        }
    }
}

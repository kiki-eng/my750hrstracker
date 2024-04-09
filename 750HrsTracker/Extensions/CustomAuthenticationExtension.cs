using _750HrsTracker.Helpers;
using _750HrsTracker.Models.Misc;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace _750HrsTracker.Extensions
{
    public static class CustomAuthenticationExtension
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
               .AddJwtBearer(options =>
               {
                   options.Events = new JwtBearerEvents
                   {

                       OnTokenValidated = context =>
                       {
                           var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                           var principal = context.Principal!;

                           //set up authorization response
                           var response = context.Response;
                           ErrorModel rhe = new ErrorModel();
                           response.ContentType = "application/json";


                           if (principal.HasClaim(c => c.Type == "Id") && principal.Identity!.IsAuthenticated)
                           {

                               var userId = Guid.Parse(principal.Claims.First(x => x.Type == "Id").Value);
                               try
                               {
                                   var user = userService.GetUserAsync(userId).Result;
                                   if (user == null)
                                   {
                                       // return unauthorized if user no longer exists
                                       context.Fail("Unathorized");
                                       rhe.message = "Unauthorized";
                                       //response.WriteAsync(JsonConvert.SerializeObject(rhe));
                                       return Task.CompletedTask;
                                   }
                                   context.Success();
                                   return Task.CompletedTask;
                               }
                               catch (Exception ex)
                               {
                                   context.Fail("Unathorized");
                                   rhe.message = $"{ex.Message}";
                                   return Task.CompletedTask;
                               }

                           }

                           context.Fail("Invalid token");
                           rhe.message = "Invalid token";
                           //response.WriteAsync(JsonConvert.SerializeObject(rhe));
                           return Task.CompletedTask;
                       }

                   };
                   options.RequireHttpsMetadata = false;
                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtSecret!)),
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero
                   };
               })
               .AddScheme<CustomPubAccessAuthenticationSchemeOption, CustomPubAccessAuthenticationHandler>
                    (CustomPubAccessAuthenticationSchemeOption.Name, op => { });

            return services;
        }
    }
}

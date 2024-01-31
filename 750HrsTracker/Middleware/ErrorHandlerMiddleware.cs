using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.Misc;
using _750HrsTracker.Services.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace _750HrsTracker.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private IEmailService _emailService;
        public async Task LogNewError(ErrorModel errorModel)
        {
            if (errorModel.severity.Equals(ErrorSeverity.HIGH))
            {
                // _emailService.SendErrorMail(errorModel.message); // TBD
            }
        }

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IEmailService emailService)
        {
            _emailService = emailService;

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                ErrorModel rhe = new ErrorModel();
                response.ContentType = "application/json";

                switch (error)
                {
                    case ApplicationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        rhe.message = $"::: {e.Message} ::: " + (error.InnerException != null ? $"{error.InnerException?.Message} :::" : "");
                        rhe.severity = ErrorSeverity.NORMAL;
                        break;

                    case ForbiddenAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        rhe.message = $"::: {e.Message} ::: " + (error.InnerException != null ? $"{error.InnerException?.Message} :::" : "");
                        rhe.severity = ErrorSeverity.HIGH;
                        break;

                    case EmailNotConfirmedException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        rhe.message = $"::: {e.Message} ::: " + (error.InnerException != null ? $"{error.InnerException?.Message} :::" : "");
                        rhe.severity = ErrorSeverity.LOW;
                        break;

                    case UnauthorizedAccessException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        rhe.message = $"::: {e.Message} ::: " + (error.InnerException != null ? $"{error.InnerException?.Message} :::" : "");
                        rhe.severity = ErrorSeverity.HIGH;
                        break;

                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        rhe.message = $"::: {e.Message} ::: " + (error.InnerException != null ? $"{error.InnerException?.Message} :::" : "");
                        rhe.severity = ErrorSeverity.LOW;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        rhe.message = $"::: {error.Message} ::: " + (error.InnerException != null ? $"{error.InnerException?.Message} :::" : ""); 
                        rhe.severity = ErrorSeverity.HIGH;
                        break;
                }

                LogNewError(rhe);
                var result = JsonConvert.SerializeObject(rhe);
                await response.WriteAsync(result);
            }
        }
    }
}

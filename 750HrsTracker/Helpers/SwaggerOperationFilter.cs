using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace _750HrsTracker.Helpers
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var actionName = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor)!.ActionName;
            var controller = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor)!.ControllerName;

            if (controller.ToLower().StartsWith("public"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "pub-access-key",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = true,
                });
            }
            


        }

    }
}

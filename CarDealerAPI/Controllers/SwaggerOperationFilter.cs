using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Controllers
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the action has the AllowAnonymous attribute
            var hasAllowAnonymous = context.ApiDescription.ActionDescriptor
                                             .EndpointMetadata
                                             .Any(em => em is AllowAnonymousAttribute);

            if (hasAllowAnonymous)
            {
                // Remove the security requirement for this operation
                operation.Security.Clear();
            }
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace RgpGame.Config.Swagger
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // var methodCustomAttributes = context.MethodInfo.CustomAttributes;
            // var isAuthorized = methodCustomAttributes.Any(attribute => attribute.AttributeType == typeof(AuthorizeAttribute));
            // // var allowAnonymous = filterPipeline.Select(info => info.Filter).Any(filter => filter is AllowAnonymousFilter);

            // if (!isAuthorized) return;

            // if (operation.Parameters == null)
            // {
            //     operation.Parameters = new List<OpenApiParameter>();
            // }

            // operation.Parameters.Add(new OpenApiParameter
            // {
            //     Name = "Authorization",
            //     In = ParameterLocation.Header,
            //     Description = "access token",
            //     Required = true,
            //     Schema = new OpenApiSchema
            //     {
            //         Type = "string",
            //         Default = new OpenApiString("Bearer "),
            //     },
            // });

             // Policy names map to scopes
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = requiredScopes.ToList()
                    }
                };
            }
        }
    }
}
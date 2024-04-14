using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Server.SwaggerConfiguration
{
	/// <summary>
	/// Operation filter to add the requirement of required header
	/// </summary>
	public class TokenHeaderParameterOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var isAuthorized = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
				context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
			var allowAnonymous = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() ||
				context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

			if (isAuthorized && !allowAnonymous)
			{
				operation.Parameters ??= new List<OpenApiParameter>();
				operation.Parameters.Add(new OpenApiParameter
				{
					Name = "Token",
					In = ParameterLocation.Header,
					Description = "token",
					Required = true,
					Schema = new OpenApiSchema
					{
						Type = "string",
						Default = new OpenApiString("")
					}
				});
			}
		}
	}
}
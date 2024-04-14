using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
	public class ETagIfMatchHeaderParameterOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var isHttpPut = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<HttpPutAttribute>().Any() ||
				context.MethodInfo.GetCustomAttributes(true).OfType<HttpPutAttribute>().Any();

			if (isHttpPut)
			{
				operation.Parameters ??= new List<OpenApiParameter>();
				operation.Parameters.Add(new OpenApiParameter
				{
					Name = "If-Match",
					In = ParameterLocation.Header,
					Description = "if etag match",
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
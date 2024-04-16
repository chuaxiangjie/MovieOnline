using GraphiQl;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Core;
using Movies.GrainClients;
using Movies.Server.Gql.App;
using Movies.Server.Infrastructure;
using Movies.Server.SwaggerConfiguration;
using System.IO;
using System;
using Movies.Contracts.Dtos;
using System.Reflection;
using System.Collections.Generic;
using Movies.Server.Controllers;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Movies.Server
{
	public class ApiStartup
	{
		private readonly IConfiguration _configuration;
		private readonly IAppInfo _appInfo;

		public ApiStartup(
			IConfiguration configuration,
			IAppInfo appInfo
		)
		{
			_configuration = configuration;
			_appInfo = appInfo;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSerilog();
			services.AddCustomAuthentication();
			services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
			{
				builder
					// .SetIsOriginAllowed((host) => true)
					.WithOrigins("http://localhost:4200")
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					;
			}));

			// note: to fix graphql for .net core 3
			services.Configure<KestrelServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});

			services.AddAppClients();
			services.AddAppGraphQL();
			services.AddControllers()
			.AddNewtonsoftJson();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddSwaggerGen(x =>
			{
				x.OperationFilter<TokenHeaderParameterOperationFilter>();
				x.OperationFilter<ETagIfMatchHeaderParameterOperationFilter>();

				var listOfTargetAssemblies = new List<Assembly> {
					typeof(CreateMovieInput).Assembly,
					typeof(MovieController).Assembly
				};

				foreach (var targetAssembly in listOfTargetAssemblies)
				{
					try
					{
						var xmlFilename = $"{targetAssembly.GetName().Name}.xml";
						x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error when includingXmlComments for swagger: {ex.Message}");
					}
				}
			}).AddSwaggerGenNewtonsoftSupport();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env
		)
		{
			app.UseCors("TempCorsPolicy");

			// add http for Schema at default url /graphql
			app.UseGraphQL<ISchema>();

			// use graphql-playground at default url /ui/playground
			app.UseGraphQLPlayground();

			//app.UseGraphQLEndPoint<AppSchema>("/graphql");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseGraphiQl();
			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Online Api V1");
			});
		}
	}
}
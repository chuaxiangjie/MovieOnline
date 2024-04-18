using GraphQL.Server;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App
{
	public static class AppGqlExtensions
	{
		public static void AddAppGraphQL(this IServiceCollection services)
		{
			services.AddGraphQL(options =>
				{
					options.EnableMetrics = true;
					options.ExposeExceptions = true;
				})
				.AddNewtonsoftJson();

			services.AddSingleton<ISchema, AppSchema>();
			services.AddSingleton<MovieGraphQuery>();
			services.AddSingleton<MovieGraphMutation>();

			services.AddSingleton<MovieGraphType>();
			services.AddSingleton<MovieBasicInfoGraphType>();
			services.AddSingleton<MoviePagedGraphType>();
			services.AddSingleton<MovieInputGraphType>();
			services.AddSingleton<MovieUpdateGraphType>();
			services.AddSingleton<GenreType>();
		}
	}
}

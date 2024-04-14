using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;

namespace Movies.GrainClients
{
	public static class GrainClientsServiceCollectionExtensions
	{
		public static void AddAppClients(this IServiceCollection services)
		{
			services.AddSingleton<IMovieGrainClient, MovieGrainClient>();
			services.AddSingleton<IMovieIndexerGrainClient, MovieIndexingGrainClient>();
		}
	}
}
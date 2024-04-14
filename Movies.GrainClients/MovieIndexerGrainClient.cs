using Movies.Contracts.Dtos;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Domain;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieIndexerGrainClient(IGrainFactory grainFactory) : IMovieIndexerGrainClient
	{
		public async Task<List<Movie>> GetAllAsync(GetMoviesInput getMoviesInput)
		{
			var searchKey = getMoviesInput.ToString();
			var movieIndexer = grainFactory.GetGrain<IMovieSearchIndexerGrain>(searchKey);

			var movies = await movieIndexer.GetManyAsync();

			return movies;
		}

		public async Task<List<Movie>> GetTopRatedAsync(GetTopRatedMoviesInput getTopRatedMoviesInput)
		{
			var movieIndexer = grainFactory.GetGrain<IMovieTopRatingIndexerGrain>(getTopRatedMoviesInput.Limit);

			var movies = await movieIndexer.GetManyAsync();

			return movies;
		}
	}
}
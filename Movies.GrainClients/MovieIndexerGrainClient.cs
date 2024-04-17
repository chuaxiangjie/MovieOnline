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
		public async Task<PagedResponseKeyset<MovieBasicInfo>> GetAllAsync(GetMoviesBasicInfoInput getMoviesBasicInfoInput)
		{
			var searchKey = getMoviesBasicInfoInput.ToString();
			var movieSearchIndexerGrain = grainFactory.GetGrain<IMovieSearchIndexerGrain>(searchKey);

			var movieBasicInfoPagedResponse = await movieSearchIndexerGrain
				.GetManyAsync(getMoviesBasicInfoInput.PageSize, getMoviesBasicInfoInput.ReferenceId);

			return movieBasicInfoPagedResponse;
		}

		public async Task<IReadOnlyList<MovieBasicInfo>> GetTopRatedAsync(GetTopRatedMoviesInput getTopRatedMoviesInput)
		{
			var movieTopRatingIndexerGrain = grainFactory
				.GetGrain<IMovieTopRatingIndexerGrain>((int)getTopRatedMoviesInput.RatingRange);
			var movies = await movieTopRatingIndexerGrain.GetManyAsync();

			return movies;
		}
	}
}
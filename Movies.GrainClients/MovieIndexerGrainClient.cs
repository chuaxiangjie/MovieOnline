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
		public async Task<PagedResponseKeyset<MovieBasicInfo>> GetAllAsync(GetSearchMoviesInput getMoviesBasicInfoInput)
		{
			var searchKey = GetSearchKey(getMoviesBasicInfoInput);
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
		
		private string GetSearchKey(GetSearchMoviesInput getMoviesBasicInfoInput)
		{
			var searchKey = "{0}_{1}";

			var name = string.IsNullOrEmpty(getMoviesBasicInfoInput.Name) ? string.Empty : getMoviesBasicInfoInput.Name.Trim();
			var genre = getMoviesBasicInfoInput.Genre is null ? string.Empty : getMoviesBasicInfoInput.Genre.ToString();

			searchKey = string.Format(searchKey, name, genre);

			return searchKey;
		}
	}
}
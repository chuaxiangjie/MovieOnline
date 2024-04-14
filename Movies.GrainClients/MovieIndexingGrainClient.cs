using Movies.Contracts;
using Movies.Domain;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieIndexingGrainClient(IGrainFactory grainFactory) : IMovieIndexingGrainClient
	{
		public async Task<List<Movie>> GetAllAsync(GetMoviesInput getMoviesInput)
		{
			var searchKey = FormatSearchKey(getMoviesInput);

			var movieIndexer = grainFactory.GetGrain<IMovieIndexingGrain>(searchKey);

			var movies = await movieIndexer.GetManyAsync();

			return movies;
		}

		private string FormatSearchKey(GetMoviesInput getMoviesInput)
		{
			var searchKey = "{0},{1}";

			var name = string.IsNullOrEmpty(getMoviesInput.Name) ? string.Empty : getMoviesInput.Name.Trim();
			var genre = getMoviesInput.Genre is null ? string.Empty : getMoviesInput.Genre.ToString();

			searchKey = string.Format(searchKey, name, genre);

			return searchKey;
		}
	}
}
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.Domain;
using Movies.Database;
using System.Linq;
using Movies.Contracts.MovieIndexerGrains;

namespace Movies.Grains
{
	public class MovieTopRatingIndexerGrain : MovieIndexerGrainBase, IMovieTopRatingIndexerGrain
	{
		private readonly IMovieRepository _movieRepository;
		private List<MovieBasicInfo> _movies;

		public MovieTopRatingIndexerGrain(IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
		}

		public override async Task OnActivateAsync()
		{
			_movies = [];

			await SubscribeToMovieCreatedOrUpdatedEventAsync(async () =>
			{
				await UpdateMovieCacheAsync();
			});

			await UpdateMovieCacheAsync();
		}

		public async Task<IReadOnlyList<MovieBasicInfo>> GetManyAsync() => await Task.FromResult(_movies.AsReadOnly());

		private async Task UpdateMovieCacheAsync()
		{
			ClearMovies();
			var numberOfTopRatedMovie = (int)this.GetPrimaryKeyLong();
			var movies = await GetFromExternalDbAsync(numberOfTopRatedMovie);

			_movies = movies;
		}

		private async Task<List<MovieBasicInfo>> GetFromExternalDbAsync(int numberOfTopRatedMovie)
		{
			var movies = await _movieRepository.GetTopRatedAsync(numberOfTopRatedMovie);

			return movies.ToList();
		}

		private void ClearMovies() => _movies.Clear();
	}
}
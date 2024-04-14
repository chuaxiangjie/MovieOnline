using Orleans;
using System;
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
		private bool _requiresMoviesRefresh = false;
		private List<Movie> _movies;

		public MovieTopRatingIndexerGrain(IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
		}

		public override async Task OnActivateAsync()
		{
			_requiresMoviesRefresh = true;
			_movies = [];

			await SubscribeToMovieCreatedOrUpdatedEventAsync(() =>
			{
				ClearMovies();
				_requiresMoviesRefresh = true;
			});
		}

		public async Task<List<Movie>> GetManyAsync()
		{
			if (_requiresMoviesRefresh)
			{
				var numberOfTopRatedMovie = GetSearchParameters();

				var movies = await _movieRepository.GetTopRatedAsync(numberOfTopRatedMovie);

				_movies = movies.ToList();
				_requiresMoviesRefresh = false;
			}
			
			return _movies;
		}

		private int GetSearchParameters()
		{
			var numberOfTopRatedMovie = this.GetPrimaryKeyLong();

			return (int)numberOfTopRatedMovie;
		}

		private void ClearMovies() => _movies.Clear();
	}
}
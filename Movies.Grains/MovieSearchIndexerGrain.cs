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
	public class MovieSearchIndexerGrain : MovieIndexerGrainBase, IMovieSearchIndexerGrain
	{
		private readonly IMovieRepository _movieRepository;
		private bool _requiresMoviesRefresh = false;
		private List<Movie> _movies;

		public MovieSearchIndexerGrain(IMovieRepository movieRepository)
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
			
			await Task.CompletedTask;
		}

		public async Task<List<Movie>> GetManyAsync()
		{
			if (_requiresMoviesRefresh)
			{
				var (name, genre) = GetSearchParameters();

				var movies = await _movieRepository.GetAllAsync(name, genre);

				_movies = movies.ToList();
				_requiresMoviesRefresh = false;
			}
			
			return _movies;
		}

		private (string name, Genre? genre) GetSearchParameters()
		{
			// <Name>,<Genre>
			var searchContentArray = this.GetPrimaryKeyString().Split(',');
			string name = searchContentArray[0];
			Genre? genre = string.IsNullOrEmpty(searchContentArray[1])
				? null
				: (Genre)Enum.Parse(typeof(Genre), searchContentArray[1]);

			return (name, genre);
		}

		private void ClearMovies() => _movies.Clear();
	}
}
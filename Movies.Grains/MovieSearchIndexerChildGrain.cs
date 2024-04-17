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
	public class MovieSearchIndexerChildGrain : MovieIndexerGrainBase, IMovieSearchIndexerChildGrain
	{
		private readonly IMovieRepository _movieRepository;
		private bool _requiresMoviesRefresh = false;
		private List<Movie> _movies;
		private PagedResponseKeyset<Movie> _moviePagedResponse;

		public MovieSearchIndexerChildGrain(IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
		}

		public override async Task OnActivateAsync()
		{
			_moviePagedResponse = new PagedResponseKeyset<Movie>(null, 0);
			_requiresMoviesRefresh = true;
			_movies = [];

			await SubscribeToMovieCreatedOrUpdatedEventAsync(() =>
			{
				ClearMovies();
				_requiresMoviesRefresh = true;
			});

			await Task.CompletedTask;
		}

		public async Task<PagedResponseKeyset<Movie>> PopulateAsync(int pageSize, int lastId)
		{
			//var (name, genre) = GetSearchParameters();

			//var moviesPagedResponse = await _movieRepository.GetAllAsync(pageSize, lastId, name, genre);

			//_moviePagedResponse = moviesPagedResponse;
			//_requiresMoviesRefresh = false;

			//return _moviePagedResponse;

			return null;
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
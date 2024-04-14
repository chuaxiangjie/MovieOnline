using Movies.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Streams;
using Movies.Domain;

namespace Movies.Grains
{
	public class MovieIndexingGrain : Grain, IMovieIndexingGrain
	{
		private bool _requiresMoviesRefresh = false;
		private List<Movie> _movies;

		public MovieIndexingGrain()
		{
			
		}

		public override async Task OnActivateAsync()
		{
			_requiresMoviesRefresh = true;
			_movies = [];

			// Create a GUID based on our GUID as a grain
			var guid = new Guid("0240de60-ddde-4b75-b183-0633966ab72e");

			// Get one of the providers which we defined in config
			var streamProvider = GetStreamProvider("SMSProvider");

			// Get the reference to a stream
			var stream = streamProvider.GetStream<MovieCreatedOrUpdatedEvent>(guid, "movieapp");

			// Set our OnNext method to the lambda which simply prints the data.
			await stream.SubscribeAsync<MovieCreatedOrUpdatedEvent>(
				async (data, token) =>
				{
					ClearMovies();
					_requiresMoviesRefresh = true;

					await Task.CompletedTask;
				});

			await Task.CompletedTask;
		}

		public async Task<List<Movie>> GetManyAsync()
		{
			if (_requiresMoviesRefresh)
			{
				var (name, genre) = GetSearchParameters();

				var movieLibraryGrain = GrainFactory.GetGrain<IMovieLibraryGrain>(0);
				var movies = await movieLibraryGrain.GetAllAsync(name, genre);

				_movies = movies;
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
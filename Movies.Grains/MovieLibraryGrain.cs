﻿using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieLibraryGrain : Grain, IMovieLibraryGrain
	{
		private readonly IPersistentState<HashSet<string>> _movieKeysState;
		private Dictionary<string, Movie> _moviesCache = [];

		public MovieLibraryGrain(
			[PersistentState(stateName: "MovieLibrary")]
			IPersistentState<HashSet<string>> movieKeysState)
		{
			_movieKeysState = movieKeysState;
		}

		public override async Task OnActivateAsync() => await PopulateMovieCacheFromStateIfRequiredAsync();

		public async Task<List<Movie>> GetAllAsync(string name = null, Genre? genre = null)
		{
			var query = _moviesCache.Values.AsEnumerable();

			if (!string.IsNullOrEmpty(name))
				query = query.Where(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);

			if (genre.HasValue)
				query = query.Where(x => x.Genres.Any(x => x == genre.Value));

			var movies = query.ToList();

			return movies;
		}

		public async Task<(bool isSuccess, string reason)> AddMovieAsync(Movie movie, bool assignAutoGeneratedId)
		{
			if (assignAutoGeneratedId)
			{
				var lastMovieId = GetLastMovieId();
				movie.Id = ++lastMovieId;
			}

			// Validate if movie key already exist
			if (_moviesCache.ContainsKey(movie.Key))
				return (false, $"{nameof(movie.Key)}:{movie.Key} already exist.");

			var movieGrain = GrainFactory.GetGrain<IMovieGrain>(movie.Key);

			await movieGrain.CreateAsync(movie);

			var movieKey = movie.Key;
			_movieKeysState.State.Add(movieKey);
			await _movieKeysState.WriteStateAsync();
			_moviesCache.Add(movieKey, movie);

			PublishMovieCreatedOrUpdatedEvent();

			return (true, null);
		}

		private async Task PopulateMovieCacheFromStateIfRequiredAsync()
		{
			if (_movieKeysState is not { State.Count: > 0 })
				return;

			var tasks = new List<Task<Movie>>();
			foreach (var movieKey in _movieKeysState.State)
			{
				tasks.Add(GrainFactory.GetGrain<IMovieGrain>(movieKey).GetAsync());
			}
			await Task.WhenAll(tasks);

			var allMovies = tasks.Select(x => x.Result);

			_moviesCache = allMovies.ToDictionary(x => x.Key, x => x);
		}

		private void PublishMovieCreatedOrUpdatedEvent()
		{
			// Pick a GUID for a chat room grain and chat room stream
			var guid = new Guid("0240de60-ddde-4b75-b183-0633966ab72e");
			// Get one of the providers which we defined in our config
			var streamProvider = GetStreamProvider("SMSProvider");
			// Get the reference to a stream
			var stream = streamProvider.GetStream<MovieCreatedOrUpdatedEvent>(guid, "movieapp");

			stream.OnNextAsync(new MovieCreatedOrUpdatedEvent());
		}

		private int GetLastMovieId()
		{
			var lastMovieId = _moviesCache.Values.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

			return lastMovieId;
		}
	}
}
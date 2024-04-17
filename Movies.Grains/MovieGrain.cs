using Movies.Contracts.Events;
using Movies.Contracts.MovieGrains;
using Movies.Database;
using Movies.Domain;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<MovieState>, IMovieGrain
	{
		private readonly IPersistentState<MovieState> _state;
		private readonly IMovieRepository _movieRepository;

		public MovieGrain(
			[PersistentState(stateName: "Movie")]
			IPersistentState<MovieState> state, IMovieRepository movieRepository)
		{
			_state = state;
			_movieRepository = movieRepository;
		}

		public override async Task OnActivateAsync()
		{
			await base.OnActivateAsync();

			if (_state.State.Movie is null)
			{
				var movie = await GetFromExternalDbAsync();
				await UpdateStateAsync(movie);
			}
		}

		public async Task<(Movie movie, string etag)> GetAsync()
		{
			var movie = _state.State.Movie;

			return await Task.FromResult((movie, _state.Etag));
		}

		public async Task<(bool isSuccess, string failureReason)> 
			CreateAsync(Movie movie)
		{
			if (_state.State.Movie is not null)
				return (false, $"{nameof(movie.Key)}:{movie.Key} already exist.");
			
			await UpdateStateAsync(movie);
			await SaveAsCreateToExternalDbAsync(movie);

			return (true, null);
		}

		public async Task<(bool isSuccess, string failureReason)> 
			UpdateAsync(string etag, string name, string description)
		{
			if (_state.State.Movie is null)
				return (false, "movie not found.");

			var currentEtag = _state.Etag;

			if (currentEtag != etag)
				return (false, "update failed, version has changed.");

			var movie = _state.State.Movie;
			movie.Name = name;
			movie.Description = description;

			await UpdateStateAsync(movie);
			await SaveAsUpdateToExternalDbAsync(movie);

			return (true, null);
		}

		private async Task UpdateStateAsync(Movie movie)
		{
			_state.State.Movie = movie;
			await _state.WriteStateAsync();

			PublishMovieCreatedOrUpdatedEvent();
		}

		private async Task SaveAsCreateToExternalDbAsync(Movie movie) => await _movieRepository.CreateAsync(movie);

		private async Task SaveAsUpdateToExternalDbAsync(Movie movie) => await _movieRepository.UpdateAsync(movie);

		private async Task<Movie> GetFromExternalDbAsync() => await _movieRepository.GetByKeyAsync(this.GetPrimaryKeyString());

		private void PublishMovieCreatedOrUpdatedEvent()
		{
			// Create a GUID based on our GUID as a grain
			var guid = new Guid(StreamConsts.StreamGuid);

			// Get one of the providers which we defined in config
			var streamProvider = GetStreamProvider(StreamConsts.StreamProvider);

			// Get the reference to a stream
			var stream = streamProvider.GetStream<MovieCreatedOrUpdatedEvent>(guid, StreamConsts.StreamNamespace);

			stream.OnNextAsync(new MovieCreatedOrUpdatedEvent());
		}
	}
}
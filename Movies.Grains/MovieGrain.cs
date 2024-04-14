using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]

	public class MovieGrain : Grain<MovieState>, IMovieGrain
	{
		private readonly IPersistentState<MovieState> _state;

		public MovieGrain(
			[PersistentState(stateName: "Movie")]
			IPersistentState<MovieState> state)
		{
			_state = state;
		}

		public async Task<Movie> GetAsync()
		{
			var movie = _state.State.Movie;

			return movie;
		}

		public async Task CreateAsync(Movie movie)
		{
			await UpdateStateAsync(movie);
		}

		public async Task UpdateAsync(string name, string description)
		{
			var movie = _state.State.Movie;
			movie.Name = name;
			movie.Description = description;

			await UpdateStateAsync(movie);
		}

		private async Task UpdateStateAsync(Movie movie)
		{
			_state.State.Movie = movie;
			await _state.WriteStateAsync();
		}
	}
}
using Movies.Contracts.MovieGrains;
using Movies.Database;
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StatelessWorker]
	public class MovieInitializerGrain : Grain, IMovieInitializerGrain
	{
		private readonly IMovieRepository _movieRepository;
		private readonly IGrainFactory _grainFactory;

		public MovieInitializerGrain(IMovieRepository movieRepository, IGrainFactory grainFactory)
		{
			_movieRepository = movieRepository;
			_grainFactory = grainFactory;
		}

		public async Task InitializeMovieGrains()
		{
			var movies = await _movieRepository.GetAllAsync();

			await Parallel.ForEachAsync(movies, async (movie, token) =>
			{
				var movieSeedingGrain = _grainFactory.GetGrain<IMovieSeedingGrain>(movie.Key);
				await movieSeedingGrain.InitFromDbAsync();
			});
		}
	}
}
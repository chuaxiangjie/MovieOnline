using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Domain;
using Orleans;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieGrainClient(IGrainFactory grainFactory) : IMovieGrainClient
	{
		public async Task<(Movie movie, string etag)> GetAsync(string movieKey)
		{
			var movieGrain = grainFactory.GetGrain<IMovieGrain>(movieKey);
			return await movieGrain.GetAsync();
		}

		public async Task<(bool isSuccess, string reason)> CreateAsync(CreateMovieInput createMovieInput)
		{
			var movieGrain = grainFactory.GetGrain<IMovieGrain>(createMovieInput.Key);

			return await movieGrain.CreateAsync(ToMovie(createMovieInput));
		}

		public async Task<(bool isSuccess, string reason)> UpdateAsync(string key, string etag, UpdateMovieInput updateMovieInput)
		{
			var movieGrain = grainFactory.GetGrain<IMovieGrain>(key);

			return await movieGrain.UpdateAsync(etag, updateMovieInput.Name, updateMovieInput.Description);
		}

		private Movie ToMovie(CreateMovieInput createMovieInput) => new Movie
		{
			Key = createMovieInput.Key,
			Name = createMovieInput.Name,
			Description = createMovieInput.Description,
			GenresAsString = string.Join(",", createMovieInput.Genres),
			Img = createMovieInput.Img,
			Length = createMovieInput.Length,
			Rate = createMovieInput.Rate
		};
	}
}
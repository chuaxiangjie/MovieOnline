using Movies.Contracts.Dtos;
using Movies.Domain;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieGrains
{
	public interface IMovieGrainClient
	{
		Task<(Movie movie, string etag)> GetAsync(string movieKey);
		Task<(bool isSuccess, string reason)> CreateAsync(CreateMovieInput createMovieInput);
		Task<(bool isSuccess, string reason)> UpdateAsync(string key, string etag, UpdateMovieInput updateMovieInput);
	}
}
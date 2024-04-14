using Movies.Domain;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrainClient
	{
		Task<Movie> GetAsync(string movieKey);
		Task<(bool isSuccess, string reason)> CreateAsync(CreateMovieInput createMovieInput);
	}
}
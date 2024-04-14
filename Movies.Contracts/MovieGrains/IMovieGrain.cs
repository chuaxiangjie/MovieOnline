using Movies.Domain;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieGrains
{
	public interface IMovieGrain : IMovieGrainBase
	{
		Task<(bool isSuccess, string failureReason)> CreateAsync(Movie movie);
		Task<(bool isSuccess, string failureReason)> UpdateAsync(string etag, string name, string description);
	}
}
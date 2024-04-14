using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task<Movie> GetAsync();
		Task CreateAsync(Movie movie);
		Task UpdateAsync(string name, string description);
	}
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieIndexingGrainClient
	{
		Task<List<Movie>> GetAllAsync(GetMoviesInput genre);
	}
}
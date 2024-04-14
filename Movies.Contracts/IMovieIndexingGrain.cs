using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieIndexingGrain : IGrainWithStringKey
	{
		Task<List<Movie>> GetManyAsync();
	}
}
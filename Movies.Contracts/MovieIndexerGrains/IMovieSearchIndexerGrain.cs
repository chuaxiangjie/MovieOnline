using Movies.Domain;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieIndexerGrains
{
	public interface IMovieSearchIndexerGrain : IGrainWithStringKey
	{
		Task<List<Movie>> GetManyAsync();
	}
}
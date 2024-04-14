using Movies.Domain;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieIndexerGrains
{
	public interface IMovieTopRatingIndexerGrain : IGrainWithIntegerKey
	{
		Task<List<Movie>> GetManyAsync();
	}
}
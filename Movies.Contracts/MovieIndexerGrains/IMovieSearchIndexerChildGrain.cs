using Movies.Domain;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieIndexerGrains
{
	public interface IMovieSearchIndexerChildGrain : IGrainWithStringKey
	{
		Task<PagedResponseKeyset<Movie>> PopulateAsync(int pageSize, int lastId);
	}
}
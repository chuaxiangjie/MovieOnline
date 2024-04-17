using Movies.Domain;
using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieIndexerGrains
{
	public interface IMovieSearchIndexerGrain : IGrainWithStringKey
	{
		Task<PagedResponseKeyset<MovieBasicInfo>> GetManyAsync(int pageSize, int referenceId);
	}
}
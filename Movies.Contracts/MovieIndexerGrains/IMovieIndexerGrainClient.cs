using Movies.Contracts.Dtos;
using Movies.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieIndexerGrains
{
	public interface IMovieIndexerGrainClient
	{
		Task<PagedResponseKeyset<MovieBasicInfo>> GetAllAsync(GetMoviesBasicInfoInput getMoviesBasicInfoInput);
		Task<IReadOnlyList<MovieBasicInfo>> GetTopRatedAsync(GetTopRatedMoviesInput getTopRatedMoviesInput);
	}
}
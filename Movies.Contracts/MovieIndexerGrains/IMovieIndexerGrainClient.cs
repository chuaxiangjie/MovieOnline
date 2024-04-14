using Movies.Contracts.Dtos;
using Movies.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieIndexerGrains
{
	public interface IMovieIndexerGrainClient
	{
		Task<List<Movie>> GetAllAsync(GetMoviesInput genre);
		Task<List<Movie>> GetTopRatedAsync(GetTopRatedMoviesInput getTopRatedMoviesInput);
	}
}
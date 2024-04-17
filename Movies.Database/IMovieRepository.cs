using Movies.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Database
{
	public interface IMovieRepository : IDisposable
	{
		Task<PagedResponseKeyset<MovieBasicInfo>> GetAllAsync(int pageSize, int referenceId, string name, Genre? genre);
		Task<IEnumerable<MovieBasicInfo>> GetTopRatedAsync(int numberOfTopRatedMovie);
		Task<Movie> GetByKeyAsync(string key);
		Task CreateAsync(Movie movie);
		Task UpdateAsync(Movie movie);
	}
}
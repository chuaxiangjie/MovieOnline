using Movies.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Database
{
	public interface IMovieRepository : IDisposable
	{
		Task<IEnumerable<Movie>> GetAllAsync(string name = null, Genre? genre = null);
		Task<IEnumerable<Movie>> GetTopRatedAsync(int numberOfTopRatedMovie);
		Task<Movie> GetByKeyAsync(string key);
		Task CreateAsync(Movie movie);
		Task UpdateAsync(Movie movie);
	}
}
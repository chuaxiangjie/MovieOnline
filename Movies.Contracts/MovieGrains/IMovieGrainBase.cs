using Movies.Domain;
using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieGrains
{
	public interface IMovieGrainBase : IGrainWithStringKey
	{
		Task<(Movie movie, string etag)> GetAsync();
	}
}
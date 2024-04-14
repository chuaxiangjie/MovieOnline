using Movies.Domain;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieGrains
{
	public interface IMovieSeedingGrain : IMovieGrainBase
	{
		Task InitFromDbAsync();
	}
}
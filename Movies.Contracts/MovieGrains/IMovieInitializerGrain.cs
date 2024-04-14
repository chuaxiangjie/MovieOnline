using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts.MovieGrains
{
	public interface IMovieInitializerGrain : IGrainWithIntegerKey
	{
		Task InitializeMovieGrains();
	}
}
using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts.Dtos
{
	public class GetTopRatedMoviesInput
	{
		[Range(1, 100)]
		public int Limit { get; set; } = 5;
	}
}
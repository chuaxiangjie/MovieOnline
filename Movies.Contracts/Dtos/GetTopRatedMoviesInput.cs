using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts.Dtos
{
	public class GetTopRatedMoviesInput
	{
		[EnumDataType(typeof(TopMovieRatingRange))]
		public TopMovieRatingRange RatingRange { get; set; } = TopMovieRatingRange.Top5;
	}
}
using Movies.Domain;

namespace Movies.Contracts.Dtos
{
	public class GetTopRatedMoviesInput
	{
		public TopMovieRatingRange RatingRange { get; set; } = TopMovieRatingRange.Top5;
	}
}
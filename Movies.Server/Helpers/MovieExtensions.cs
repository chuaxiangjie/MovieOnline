using Movies.Contracts.Dtos;
using Movies.Domain;

namespace Movies.Server.Mappers
{
	public static class MovieExtensions
	{
		public static MovieOutput ToMovieOutput(this Movie movie, string etag = null)
		{
			if (movie is null)
				return null;

			return new MovieOutput
			{
				Id = movie.Id,
				Key = movie.Key,
				Name = movie.Name,
				Description = movie.Description,
				Genres = movie.Genres,
				Length = movie.Length,
				Rate = movie.Rate,
				Img = movie.Img,
				Meta = new MovieMeta
				{
					ETag = etag
				}
			};
		}
	}
}

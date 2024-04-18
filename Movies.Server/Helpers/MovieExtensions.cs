using Movies.Contracts.Dtos;
using Movies.Domain;
using System.Linq;

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

		public static MovieBasicInfoOutput ToMovieBasicInfoOutput(this MovieBasicInfo movieBasicInfo)
		{
			if (movieBasicInfo is null)
				return null;

			return new MovieBasicInfoOutput
			{
				Id = movieBasicInfo.Id,
				Key = movieBasicInfo.Key,
				Name = movieBasicInfo.Name,
				Genres = movieBasicInfo.Genres,
				Rate = movieBasicInfo.Rate
			};
		}

		public static MoviePagedOutput<MovieOutput> ToMoviePagedOutput(this PagedResponseKeyset<Movie> pagedResponse)
		{
			var output = new MoviePagedOutput<MovieOutput>
			{
				ReferenceId = pagedResponse.ReferenceId,
				Movies = pagedResponse.Data.Select(x => x.ToMovieOutput()).ToList()
			};

			return output;
		}

		public static MoviePagedOutput<MovieBasicInfoOutput> ToMoviePagedOutput(this PagedResponseKeyset<MovieBasicInfo> pagedResponse)
		{
			var output = new MoviePagedOutput<MovieBasicInfoOutput>
			{
				ReferenceId = pagedResponse.ReferenceId,
				Movies = pagedResponse.Data.Select(x => x.ToMovieBasicInfoOutput()).ToList()
			};

			return output;
		}
	}
}
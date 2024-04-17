using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Domain;
using Movies.Server.Gql.Types;
using Movies.Server.Helpers;
using Movies.Server.Mappers;
using System.Linq;

namespace Movies.Server.Gql.App
{
	public class MovieGraphQuery : ObjectGraphType
	{
		public MovieGraphQuery(IMovieGrainClient movieGrainClient, IMovieIndexerGrainClient movieIndexerGrainClient)
		{
			Name = nameof(MovieGraphQuery);
			Field<MovieGraphType>(
				name: "getmovie",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "key"
				}),
				resolve: ctx =>
				{
					var (movie, etag) = movieGrainClient.GetAsync(ctx.Arguments["key"].ToString()).Result;

					var output = movie.ToMovieOutput(etag);

					return output;
				}
			);

			Field<ListGraphType<MovieGraphType>>(
				name: "getmovies",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "genre"
				}, new QueryArgument<StringGraphType>
				{
					Name = "name"
				}),
				resolve: ctx =>
				{
					var genreString = ctx.GetArgument<string>("genre", null);
					Genre? genre = string.IsNullOrEmpty(genreString) ? null : EnumHelper.ParseEnum<Genre>(genreString);

					var name = ctx.GetArgument<string>("name", null);

					var getMoviesBasicInfoInput = new GetMoviesBasicInfoInput
					{
						Genre = genre,
						Name = name
					};

					var movies = movieIndexerGrainClient.GetAllAsync(getMoviesBasicInfoInput).Result;

					//var output = movies.Select(x => x.ToMovieOutput());

					//return output;

					return null;
				}
			);

			Field<ListGraphType<MovieGraphType>>(
				name: "gettopratedmovies",
				arguments: new QueryArguments(new QueryArgument<BigIntGraphType>
				{
					Name = "ratingRange"
				}),
				resolve: ctx =>
				{
					var ratingRangeInt = ctx.GetArgument<int>("ratingRange", (int)TopMovieRatingRange.Top5);
					var ratingRange = (TopMovieRatingRange)ratingRangeInt;

					var getTopRatedMoviesInput = new GetTopRatedMoviesInput
					{
						RatingRange = ratingRange
					};

					var movies = movieIndexerGrainClient.GetTopRatedAsync(getTopRatedMoviesInput).Result;

					var output = movies.Select(x => x.ToMovieBasicInfoOutput());

					return output;
				}
			);
		}
	}
}
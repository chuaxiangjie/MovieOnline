using Azure;
using GraphQL.Types;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Server.Gql.Types;
using Movies.Server.Mappers;

namespace Movies.Server.Gql.App
{
	public class MovieGraphQuery : ObjectGraphType
	{
		public MovieGraphQuery(IMovieGrainClient movieGrainClient)
		{
			Name = nameof(MovieGraphQuery);
			Field<MovieGraphType>("getmovies",
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
		}
	}
}
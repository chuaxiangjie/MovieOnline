using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;
using System;

namespace Movies.Server.Gql.App
{
	public class MovieGraphQuery : ObjectGraphType
	{
		public MovieGraphQuery(IMovieGrainClient sampleClient)
		{
			Name = nameof(MovieGraphQuery);
			Field<MovieGraphType>("getmovies",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "key"
				}),
				resolve: ctx => sampleClient.GetAsync(ctx.Arguments["key"].ToString()).Result
			);
		}
	}
}
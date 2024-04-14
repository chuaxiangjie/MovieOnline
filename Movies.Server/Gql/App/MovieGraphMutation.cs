using GraphQL;
using GraphQL.Types;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Server.Gql.Types;
using System;

namespace Movies.Server.Gql.App
{
	public class MovieGraphMutation : ObjectGraphType
	{
		public MovieGraphMutation(IMovieGrainClient movieGrainClient, IMovieIndexerGrainClient movieIndexerGrainClient)
		{
			Name = nameof(MovieGraphMutation);
			Field<MovieGraphType>(
				name: "createMovie",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<MovieInputGraphType>>
				{
					Name = "createMovieInput"
				}),
				resolve: ctx =>
				{
					var input = ctx.GetArgument<CreateMovieInput>("createMovieInput");

					var (isSuccess, reason) = movieGrainClient.CreateAsync(input).Result;

					if (!isSuccess)
						throw new Exception(reason);

					return null;
				}
			);
		}
	}
}
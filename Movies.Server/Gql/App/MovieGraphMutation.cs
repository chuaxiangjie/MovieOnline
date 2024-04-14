using Azure;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Server.Gql.Types;
using Movies.Server.Helpers;
using System;

namespace Movies.Server.Gql.App
{
	public class MovieGraphMutation : ObjectGraphType
	{
		public MovieGraphMutation(
			IMovieGrainClient movieGrainClient, 
			IMovieIndexerGrainClient movieIndexerGrainClient,
			IHttpContextAccessor httpContextAccessor)
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

			Field<MovieGraphType>(
				name: "updateMovie",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<MovieUpdateGraphType>>
				{
					Name = "updateMovieInput"
				}, 
				new QueryArgument<NonNullGraphType<StringGraphType>>
				{
					Name = "key"
				}),
				resolve: ctx =>
				{
					var input = ctx.GetArgument<UpdateMovieInput>("updateMovieInput");
					var key = ctx.GetArgument<string>("key");
					var etag = httpContextAccessor.HttpContext.Request.GetETag();

					var (isSuccess, reason) = movieGrainClient.UpdateAsync(key, etag, input).Result;

					if (!isSuccess)
						throw new Exception(reason);

					return null;
				}
			);
		}
	}
}
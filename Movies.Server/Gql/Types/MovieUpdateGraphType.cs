using GraphQL.Types;
using Movies.Contracts.Dtos;

namespace Movies.Server.Gql.Types
{
	public class MovieUpdateGraphType : InputObjectGraphType<UpdateMovieInput>
	{
		public MovieUpdateGraphType()
		{
			Name = "movieupdate";
			Description = "A movie api";

			Field(x => x.Name).Description("Name.");
			Field(x => x.Description).Description("Description.");
		}
	}
}
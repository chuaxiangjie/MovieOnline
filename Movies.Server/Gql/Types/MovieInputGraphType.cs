using GraphQL.Types;
using Movies.Contracts.Dtos;

namespace Movies.Server.Gql.Types
{
	public class MovieInputGraphType : InputObjectGraphType<CreateMovieInput>
	{
		public MovieInputGraphType()
		{
			Name = "movieinput";
			Description = "A movie api";

			Field(x => x.Key).Description("Key.");
			Field(x => x.Name).Description("Name.");
			Field(x => x.Description).Description("Description.");
			Field<ListGraphType<GenreType>>("Genres", resolve: context => context.Source.Genres);
			Field(x => x.Rate).Description("Rating.");
			Field(x => x.Length).Description("Length.");
			Field(x => x.Img).Description("Image.");
		}
	}
}
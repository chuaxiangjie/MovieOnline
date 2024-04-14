using GraphQL.Types;
using Movies.Domain;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphType : ObjectGraphType<Movie>
	{
		public MovieGraphType()
		{
			Name = "Movie";
			Description = "A movie api";

			Field(x => x.Id).Description("Unique key.");
			Field(x => x.Key).Description("Key.");
			Field(x => x.Name).Description("Name.");
			Field(x => x.Description).Description("Description.");
			Field(x => x.Genres).Description("Genres.");
			Field(x => x.Rate).Description("Rate.");
			Field(x => x.Length).Description("Length.");
			Field(x => x.Img).Description("Image.");
		}
	}
}
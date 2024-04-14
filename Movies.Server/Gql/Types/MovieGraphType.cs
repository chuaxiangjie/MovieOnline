using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphType : ObjectGraphType<MovieDto>
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
			Field(x => x.Rating).Description("Rating.");
			Field(x => x.Duration).Description("Duration.");
			Field(x => x.Image).Description("Image.");
		}
	}
}
using GraphQL.Types;
using Movies.Contracts.Dtos;
using Movies.Domain;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphType : ObjectGraphType<MovieOutput>
	{
		public MovieGraphType()
		{
			Name = "Movie";
			Description = "A movie api";

			Field(x => x.Id).Description("Unique key.");
			Field(x => x.Key).Description("Key.");
			Field(x => x.Name).Description("Name.");
			Field(x => x.Description).Description("Description.");
			Field<ListGraphType<GenreType>>("Genres", resolve: context => context.Source.Genres);
			Field(x => x.Rate).Description("Rating.");
			Field(x => x.Length).Description("Length.");
			Field(x => x.Img).Description("Image.");
			Field(x => x.Meta.ETag).Description("Etag.");
		}
	}

	public class GenreType : EnumerationGraphType<Genre>
	{
		public GenreType()
		{
			Name = "Genre";
		}
	}
}
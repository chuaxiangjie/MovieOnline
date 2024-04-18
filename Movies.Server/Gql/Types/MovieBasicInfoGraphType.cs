using GraphQL.Types;
using Movies.Contracts.Dtos;

namespace Movies.Server.Gql.Types
{
	public class MovieBasicInfoGraphType : ObjectGraphType<MovieBasicInfoOutput>
	{
		public MovieBasicInfoGraphType()
		{
			Name = "MovieBasicInfo";
			Description = "A movie api";

			Field(x => x.Id).Description("Unique key.");
			Field(x => x.Key).Description("Key.");
			Field(x => x.Name).Description("Name.");
			Field<ListGraphType<GenreType>>("Genres", resolve: context => context.Source.Genres);
			Field(x => x.Rate).Description("Rating.");
		}
	}
}
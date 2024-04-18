using GraphQL.Types;
using Movies.Contracts.Dtos;

namespace Movies.Server.Gql.Types
{
	public class MoviePagedGraphType : ObjectGraphType<MoviePagedOutput<MovieBasicInfoOutput>>
	{
		public MoviePagedGraphType()
		{
			Name = "MoviePagination";
			Description = "A movie api";

			Field(x => x.ReferenceId).Description("Reference Id");
			Field<ListGraphType<MovieBasicInfoGraphType>>("Movies", resolve: context => context.Source.Movies);
		}
	}
}
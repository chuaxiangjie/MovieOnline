using GraphQL.Types;
using Movies.Domain;

namespace Movies.Server.Gql.Types
{
	public class GenreType : EnumerationGraphType<Genre>
	{
		public GenreType()
		{
			Name = "Genre";
		}
	}
}
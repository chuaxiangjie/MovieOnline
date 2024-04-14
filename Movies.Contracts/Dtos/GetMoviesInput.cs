using Movies.Domain;

namespace Movies.Contracts.Dtos
{
	public class GetMoviesInput
	{
		public Genre? Genre { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			var searchKey = "{0},{1}";

			var name = string.IsNullOrEmpty(Name) ? string.Empty : Name.Trim();
			var genre = Genre is null ? string.Empty : Genre.ToString();

			searchKey = string.Format(searchKey, name, genre);

			return searchKey;
		}
	}
}
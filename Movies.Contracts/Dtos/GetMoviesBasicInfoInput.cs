using Movies.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts.Dtos
{
	public class GetMoviesBasicInfoInput
	{
		[EnumDataType(typeof(Genre))]
		public Genre? Genre { get; set; }
		public string Name { get; set; }
		public int ReferenceId { get; set; }
		public int PageSize { get; set; } = 100;

		public override string ToString()
		{
			var searchKey = "{0}_{1}";

			var name = string.IsNullOrEmpty(Name) ? string.Empty : Name.Trim();
			var genre = Genre is null ? string.Empty : Genre.ToString();

			searchKey = string.Format(searchKey, name, genre);

			return searchKey;
		}
	}
}
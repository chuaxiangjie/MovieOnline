using Movies.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts.Dtos
{
	public class GetSearchMoviesInput
	{
		[EnumDataType(typeof(Genre))]
		public Genre? Genre { get; set; }
		public string Name { get; set; }
		public int ReferenceId { get; set; }
		public int PageSize { get; set; } = 100;
	}
}
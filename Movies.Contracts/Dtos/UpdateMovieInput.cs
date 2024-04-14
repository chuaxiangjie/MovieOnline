using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts.Dtos
{
	public class UpdateMovieInput
	{
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Required]
		[MaxLength(500)]
		public string Description { get; set; }
	}
}
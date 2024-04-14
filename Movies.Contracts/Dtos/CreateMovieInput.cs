using Movies.Contracts.Attributes;
using Movies.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts.Dtos
{
	public class CreateMovieInput
	{
		[Required]
		[MaxLength(100)]
		[RegularExpression(@"^[^,]*$",
			ErrorMessage = "comma is not allowed.")]
		public string Key { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Required]
		[MaxLength(500)]
		public string Description { get; set; }

		[Required]
		[MinLength(1, ErrorMessage = "Minimum one type of Genre is required.")]
		[EnumArrayDataTypeWithNoDuplicates]
		public Genre[] Genres { get; set; }

		[RegularExpression(@"^(?<![\d\.-])\d(\.\d)?(?!(\.\d)|\d)|(?<![\d\.-])10(?!(\.\d)|\d])$", 
			ErrorMessage = "Rate must be between 0.0 and 10 and of 1 decimal place (optional)")]
		public decimal Rate { get; set; }

		[Required]
		[MaxLength(50)]
		public string Length { get; set; }

		[Required]
		[MaxLength(255)]
		public string Img { get; set; }
	}
}
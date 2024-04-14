using Movies.Domain;

namespace Movies.Contracts.Dtos
{
	public class CreateMovieInput
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Genre[] Genres { get; set; }
		public decimal Rate { get; set; }
		public string Length { get; set; }
		public string Img { get; set; }
	}
}
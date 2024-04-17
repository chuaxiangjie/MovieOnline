using Movies.Domain;

namespace Movies.Contracts.Dtos
{
	public class MovieBasicInfoOutput
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public Genre[] Genres { get; set; }
		public decimal Rate {  get; set; } 
	}
}
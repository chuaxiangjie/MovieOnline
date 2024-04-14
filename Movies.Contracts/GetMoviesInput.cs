using Movies.Domain;

namespace Movies.Contracts
{
	public class GetMoviesInput
	{
		public Genre? Genre { get; set; }
		public string Name { get; set; }
	}
}
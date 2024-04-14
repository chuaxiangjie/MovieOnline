namespace Movies.Contracts
{
	public class CreateMovieInput
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Genre[] Genres { get; set; }
		public decimal Rating { get; set; }

		// In Minutes
		public int Duration { get; set; }
		public string Image { get; set; }
	}
}
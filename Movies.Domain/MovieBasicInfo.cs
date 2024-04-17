namespace Movies.Domain
{
	public class MovieBasicInfo : Entity<int>
	{
		public string Key { get; set; }
		public string Name { get; set; }
		public Genre[] Genres { get;set; }
		public decimal Rate { get; set; }
	}
}
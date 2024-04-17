namespace Movies.Contracts.Dtos
{
	public class MovieOutput : MovieBasicInfoOutput
	{
		public string Description { get; set; }
		public string Length { get; set; }
		public string Img { get; set; }
		public MovieMeta Meta { get; set; }
	}

	public class MovieMeta
	{
		public string ETag { get; set; }
	}
}
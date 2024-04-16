using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Movies.Domain
{
	public class Movie
	{
		public int Id { get; set; }

		public string Key { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
		public Genre[] Genres { get;set; }
		[NotMapped]
		public string GenresAsString { get; set; }

		public decimal Rate { get; set; }

		public string Length { get; set; }

		public string Img { get; set; }
	}
}
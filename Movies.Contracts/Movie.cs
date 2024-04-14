using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movies.Contracts
{
	public class Movie
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Key { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		public Genre[] Genres { get; set; }

		[Required]
		public decimal Rating { get; set; }

		// In Minutes
		[Required]
		public int Duration { get; set; }

		[Required]
		public string Image { get; set; }
	}

	public class MovieLibrary
	{
		public List<Movie> Movies { get; set; }	
	}
}
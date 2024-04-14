﻿using System;
using System.Linq;

namespace Movies.Domain
{
	public class Movie
	{
		public int Id { get; set; }

		public string Key { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Genre[] Genres => GenresAsString
			.Split(',')
			.Select(x => (Genre)Enum.Parse(typeof(Genre), x))
			.ToArray();

		public string GenresAsString { get; set; }

		public decimal Rate { get; set; }

		public string Length { get; set; }

		public string Img { get; set; }
	}
}
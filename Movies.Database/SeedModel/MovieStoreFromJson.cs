﻿using Movies.Domain;
using System.Collections.Generic;

namespace Movies.Database.SeedModel
{
	public class MovieStoreFromJson
	{
		public List<MovieFromJson> Movies { get; set; }
	}

	public class MovieFromJson
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
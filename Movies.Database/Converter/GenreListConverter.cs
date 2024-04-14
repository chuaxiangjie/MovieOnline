using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movies.Domain;
using System;
using System.Linq;

namespace Movies.Database.Converter
{
	public class GenreListConverter : ValueConverter<Genre[], string>
	{
		public GenreListConverter()
			: base(
				  v => string.Join(",", v.Select(g => g.ToString())),
				  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
					.Select(g => (Genre)Enum.Parse(typeof(Genre), g))
					.ToList().ToArray()
			  )
		{
		}
	}
}
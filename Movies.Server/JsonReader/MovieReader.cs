using Movies.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Movies.Server.JsonReader
{
	public class MovieReader
	{
		public async Task<List<Movie>> ReadAsync()
		{
			var serializer = new JsonSerializer();
		
			try
			{
				using var streamReader = new StreamReader("../movies.json");
				using var textReader = new JsonTextReader(streamReader);
				var movieLibrary = serializer.Deserialize<MovieLibrary>(textReader);
				return movieLibrary.Movies;
			}
			catch (Exception ex)
			{

			}

			return null;
		}
	}
}
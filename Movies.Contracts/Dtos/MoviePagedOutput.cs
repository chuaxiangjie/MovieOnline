using System.Collections.Generic;

namespace Movies.Contracts.Dtos
{
	public class MoviePagedOutput<T>
	{
		public int Reference { get; set; }
		public List<T> Movies { get; set; }
	}
}
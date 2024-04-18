using System.Collections.Generic;

namespace Movies.Contracts.Dtos
{
	public class MoviePagedOutput<T>
	{
		public int ReferenceId { get; set; }
		public IReadOnlyList<T> Movies { get; set; }
	}
}
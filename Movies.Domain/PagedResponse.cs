using System.Collections.Generic;

namespace Movies.Domain
{
	public record PagedResponseKeyset<T>
	{
		public int ReferenceId { get; set; }
		public List<T> Data { get; set; }

		public PagedResponseKeyset(List<T> data, int referenceId)
		{
			Data = data;
			ReferenceId = referenceId;
		}
	}
}
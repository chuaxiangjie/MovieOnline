using System.Collections.Generic;

namespace Movies.Domain
{
	public record PagedResponseKeyset<T>
	{
		public int ReferenceId { get; set; }
		public IReadOnlyList<T> Data { get; set; }

		public PagedResponseKeyset(IReadOnlyList<T> data, int referenceId)
		{
			Data = data;
			ReferenceId = referenceId;
		}
	}
}
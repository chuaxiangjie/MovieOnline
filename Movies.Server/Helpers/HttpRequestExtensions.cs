using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Movies.Server.Helpers
{
	public static class HttpRequestExtensions
	{
		const string MATCH_HEADER = "If-Match";

		public static string GetETag(this HttpRequest httpRequest)
		{
			if (httpRequest.Headers.TryGetValue(MATCH_HEADER, out var value))
				return value.FirstOrDefault();

			return default;
		}
	}
}
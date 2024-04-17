using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Movies.Contracts
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TopMovieRatingRange
	{
		Top5 = 5,
		Top10 = 10,
		Top20 = 20,
		Top30 = 30,
		Top50 = 50,
		Top100 = 50,
		Top200 = 200
	}
}
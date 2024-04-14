using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Movies.Domain
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Genre
	{
		Action = 1,
		Adventure,
		Comedy,
		Crime,
		Biography,
		Drama,
		History,
		Sport,
		Mystery,
		Thriller,
		Scifi
	}
}
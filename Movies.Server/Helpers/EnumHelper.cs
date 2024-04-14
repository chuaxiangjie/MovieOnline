using System;

namespace Movies.Server.Helpers
{
	public static class EnumHelper
	{
		public static T ParseEnum<T>(string enumString) where T : struct
		{
			return Enum.TryParse(enumString, true, out T result)
				? result
				: throw new ArgumentException($"[{enumString}] can't be convert to {typeof(T).Name}");
		}
	}
}

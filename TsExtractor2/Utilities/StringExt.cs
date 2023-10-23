using System;

namespace TsExtractor2.Utilities
{
	public static class StringExt
	{
		public static string ProperCase(this string inp)
		{
			if (string.IsNullOrWhiteSpace(inp))
				return "";

			inp = inp.Trim();

			string result;

			if (inp.StartsWith("_"))
				result = "_" + ProperCase(inp[1..]);
			else
				result = string.Concat(inp[..1].ToUpper(), inp.AsSpan(1, inp.Length - 1));

			return result;
		}

		public static string CamelCase(this string inp)
		{
			if (string.IsNullOrWhiteSpace(inp))
				return "";

			inp = inp.Trim();

			string result;

			if (inp.StartsWith("_"))
				result = "_" + CamelCase(inp[1..]);
			else
				result = inp[..1].ToLower() + inp[1..];

			return result;
		}
	}
}

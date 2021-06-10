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
				result = "_" + ProperCase(inp.Substring(1, inp.Length - 1));
			else
				result = inp.Substring(0, 1).ToUpper() + inp.Substring(1, inp.Length - 1);

			return result;
		}

		public static string CamelCase(this string inp)
		{
			if (string.IsNullOrWhiteSpace(inp))
				return "";

			inp = inp.Trim();

			string result;

			if (inp.StartsWith("_"))
				result = "_" + CamelCase(inp.Substring(1, inp.Length - 1));
			else
				result = inp.Substring(0, 1).ToLower() + inp.Substring(1, inp.Length - 1);

			return result;
		}
	}
}

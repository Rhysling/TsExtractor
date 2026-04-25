using System;
using System.Collections.Generic;
using System.Linq;

namespace TsExtractor2.Utilities
{
	public static class ArgValues
	{
		private static Dictionary<string, String> argDict;

		public static void LoadArgs(string[] args)
		{
			argDict = [];

			if (args == null || args.Length == 0) return;

			argDict = args
				.Where(a => a.Contains('='))
				.Select(a => a.Split('='))
				.ToDictionary(k => k[0].ToLower(), v => v[1]);
		}

		public static string SourcePath => argDict.TryGetValue("sourcepath", out string value) ? value : null;
		public static string OutPath => argDict.TryGetValue("outpath", out string value) ? value : null;
		public static string[] ExcludeProjectNames => argDict.TryGetValue("excludeprojectnames", out string value) ? value.Split(',') : null;
	}
}

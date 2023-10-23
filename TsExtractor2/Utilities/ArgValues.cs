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
			argDict = new Dictionary<string, string>();

			if (args == null || !args.Any()) return;

			argDict = args
				.Where(a => a.Contains('='))
				.Select(a => a.Split('='))
				.ToDictionary(k => k[0].ToLower(), v => v[1]);
		}

		public static string SourcePath => argDict.ContainsKey("sourcepath") ? argDict["sourcepath"] : null;
		public static string OutPath => argDict.ContainsKey("outpath") ? argDict["outpath"] : null;
		public static string[] ExcludeProjectNames => argDict.ContainsKey("excludeprojectnames") ? argDict["excludeprojectnames"].Split(',') : null;
	}
}

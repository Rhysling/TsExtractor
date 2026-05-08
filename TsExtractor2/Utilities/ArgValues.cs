using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TsExtractor2.Utilities
{
	public static class ArgValues
	{
		private static Dictionary<string, string> argDict = [];

		public static void LoadFromArgs(string[] args)
		{
			if (args == null || args.Length == 0) return;

			argDict = args
				.Where(a => a.Contains('='))
				.Select(a => a.Split('='))
				.ToDictionary(k => k[0].ToLower(), v => v[1]);
		}

		public static void LoadFromFile()
		{
			// Settings -- From File/Args ********************************
			string settingsFile = Path.Combine(AppContext.BaseDirectory, "settings.txt");

			if (!File.Exists(settingsFile)) return;

			string tabDelimArgs = File.ReadAllText(settingsFile);

			if (String.IsNullOrWhiteSpace(tabDelimArgs)) return;

			string[] lines = tabDelimArgs.Split(separator, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in lines)
			{
				var kvp = line.Split('\t');
				if (kvp.Length > 1) argDict[kvp[0].ToLower()] = kvp[1];
			}
		}

		public static string? SourcePath => argDict.TryGetValue("sourcepath", out string? value) ? value : null;
		public static string? OutPath => argDict.TryGetValue("outpath", out string? value) ? value : null;
		public static string[] ExcludeProjectNames => argDict.TryGetValue("excludeprojectnames", out string? value) ? value.Split(',') : [];

		private static readonly string[] separator = ["\r\n", "\n"];
	}
}

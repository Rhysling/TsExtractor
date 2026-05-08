using System.IO;
using System.Linq;

namespace TsExtractor2.Utilities
{
	public static class Utils
	{
		public static string? FindSlnPath(string? currentPath, int iteration)
		{
			int itCount = iteration + 1;
			if (itCount > 6) return null;

			string? parentPath = (currentPath != null) ? Directory.GetParent(currentPath)?.FullName : null;
			string[] slnFiles = (parentPath != null) ? Directory.GetFiles(parentPath, "*.sln") : [];

			if (slnFiles.Any())
				return slnFiles[0];
			else
				return FindSlnPath(parentPath, itCount);
		}
	}
}

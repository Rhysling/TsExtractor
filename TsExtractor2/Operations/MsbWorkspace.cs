using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using TsExtractor2.Models;
using TsExtractor2.Utilities;

namespace TsExtractor2.Operations
{
	public static class MsbWorkspace
	{
		private static VisualStudioInstance vsInstance;


		public static string InitWorkspace()
		{
			vsInstance = MSBuildLocator.QueryVisualStudioInstances().ToArray()[0];
			MSBuildLocator.RegisterInstance(vsInstance);
			return $"// Using MSBuild ver. {vsInstance.Version} to load projects.\r\n// Generated - {DateTime.Now:yyyy/MM/dd-HH:mm:ss}";
		}

		public static SolutionModel GetCompilations(string sourceFullPath, string[] excludeProjectNames = null)
		{
			var sm = new SolutionModel();
			excludeProjectNames ??= Array.Empty<string>();

			if (sourceFullPath == null || sourceFullPath.EndsWith(".sln"))
			{
				sourceFullPath ??= Utils.FindSlnPath(AppDomain.CurrentDomain.BaseDirectory, 0);

				using var workspace = MSBuildWorkspace.Create();
				var solution = workspace.OpenSolutionAsync(sourceFullPath).Result;

				sm.SolutionName = System.IO.Path.GetFileNameWithoutExtension(solution.FilePath);
				sm.Projects = solution.Projects
					.Where(a => !excludeProjectNames.Contains(a.Name))
					.Select(a => new ProjectModel(a.Name, a.GetCompilationAsync().Result))
					.ToList();
			}
			else if (sourceFullPath.EndsWith(".csproj"))
			{
				sm.SolutionName = "Single Project";
				using var workspace = MSBuildWorkspace.Create();
				var project = workspace.OpenProjectAsync(sourceFullPath).Result;
				sm.Projects = new List<ProjectModel> { new ProjectModel(project.Name, project.GetCompilationAsync().Result) };
			}
			else
			{
				Console.WriteLine("'SourcePath' must be a 'sln; or 'csproj' file.");
				throw new ArgumentException("'SourcePath' must be a 'sln; or 'csproj' file.");
			}

			return sm;
		}

	}
}

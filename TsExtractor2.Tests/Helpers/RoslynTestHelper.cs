using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TsExtractor2.Models;

namespace TsExtractor2.Tests.Helpers;

/// <summary>
/// Builds Roslyn-backed model objects from in-memory C# source strings so that
/// tests can exercise TreeModel, SolutionModel, and other Roslyn-dependent code
/// without MSBuild or a real project file.
/// </summary>
internal static class RoslynTestHelper
{
	internal static ProjectModel BuildProjectModel(string source, string projectName = "TestProject")
	{
		var tree = CSharpSyntaxTree.ParseText(source);

		// System.Private.CoreLib supplies Object, Int32, String, Attribute, List<T>, etc.
		var refs = new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };

		var compilation = CSharpCompilation.Create(
			projectName,
			[tree],
			refs,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		return new ProjectModel(projectName, compilation);
	}

	internal static SolutionModel BuildSolutionModel(string source, string solutionName = "TestSolution")
	{
		return new SolutionModel
		{
			SolutionName = solutionName,
			Projects = [BuildProjectModel(source)]
		};
	}
}

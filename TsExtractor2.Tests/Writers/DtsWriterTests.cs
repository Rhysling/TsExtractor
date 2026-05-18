using System.Reflection;
using TsExtractor2.Models;
using TsExtractor2.Writers;

namespace TsExtractor2.Tests.Writers;

// DtsWriter is tested by injecting pre-built ClassModels directly into SolutionModel
// via reflection (bypassing the Roslyn compilation path). This keeps each test focused
// on a single rendering concern without re-testing the extraction pipeline.

public class DtsWriterTests
{
	[Fact]
	public void Write_RegularClass_UsesTypeKeyword()
	{
		var output = RunWriter([MakeClass("MyModel", isInterface: false, "Id", "Int32")]);

		Assert.Contains("type MyModel = {", output);
	}

	[Fact]
	public void Write_InterfaceClass_UsesInterfaceKeywordWithIPrefix()
	{
		var output = RunWriter([MakeClass("ContactVM", isInterface: true, "Email", "String")]);

		Assert.Contains("interface IContactVM {", output);
	}

	[Fact]
	public void Write_PropertyName_IsCamelCased()
	{
		var output = RunWriter([MakeClass("MyModel", isInterface: false, "FirstName", "String")]);

		Assert.Contains("firstName: string;", output);
	}

	[Fact]
	public void Write_Header_AppearsAtTopOfFile()
	{
		const string header = "// test header line";
		var sm = InjectTsClasses([], "MySolution");
		var tempFile = Path.GetTempFileName();
		try
		{
			new DtsWriter(tempFile, header, sm).Write();
			var lines = File.ReadAllLines(tempFile);
			Assert.Equal(header, lines[0]);
		}
		finally { File.Delete(tempFile); }
	}

	[Fact]
	public void Write_SolutionName_AppearsInOutput()
	{
		var sm = InjectTsClasses([], "AwesomeSolution");
		var output = RunWriter(sm);

		Assert.Contains("// SOLUTION: AwesomeSolution", output);
	}

	// ── helpers ─────────────────────────────────────────────────────────────

	private static ClassModel MakeClass(string className, bool isInterface, string propName, string propTypeName)
	{
		return new ClassModel
		{
			FilePath = "test.cs",
			NamespaceName = "Test",
			ClassName = className,
			IsTypescriptModel = true,
			IsInterface = isInterface,
			TsName = isInterface ? "I" + className : className,
			PropertyList =
			[
				new PropModel
				{
					PropName = propName,
					PropTypes = new PropTypeCollection { TypeName = propTypeName }
				}
			]
		};
	}

	// Injects a pre-built class list into SolutionModel, bypassing the Roslyn pipeline.
	private static SolutionModel InjectTsClasses(List<ClassModel> classes, string solutionName = "TestSolution")
	{
		var sm = new SolutionModel { SolutionName = solutionName, Projects = [] };
		typeof(SolutionModel)
			.GetField("tsClasses", BindingFlags.NonPublic | BindingFlags.Instance)!
			.SetValue(sm, classes);
		return sm;
	}

	private static string RunWriter(List<ClassModel> classes, string solutionName = "TestSolution")
		=> RunWriter(InjectTsClasses(classes, solutionName));

	private static string RunWriter(SolutionModel sm)
	{
		string tempFile = Path.GetTempFileName();
		try
		{
			new DtsWriter(tempFile, "// header", sm).Write();
			return File.ReadAllText(tempFile);
		}
		finally { File.Delete(tempFile); }
	}
}

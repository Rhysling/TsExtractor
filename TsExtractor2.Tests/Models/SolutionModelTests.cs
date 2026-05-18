using TsExtractor2.Tests.Helpers;

namespace TsExtractor2.Tests.Models;

// These tests build an in-memory Roslyn compilation from a C# source string so the
// full SolutionModel.TsClasses pipeline (TreeModel → ProjectModel → SolutionModel) runs
// without MSBuild. TypeScriptModelAttribute is defined inline in each source string
// to keep semantic analysis self-contained.

public class SolutionModelTests
{
	[Fact]
	public void TsClasses_OnlyIncludesClassesMarkedWithAttribute()
	{
		var source = """
			namespace Test;
			public class TypeScriptModelAttribute : System.Attribute { }
			[TypeScriptModel]
			public class MarkedClass { }
			public class UnmarkedClass { }
			""";

		var tsClasses = RoslynTestHelper.BuildSolutionModel(source).TsClasses;

		Assert.Contains(tsClasses, c => c.ClassName == "MarkedClass");
		Assert.DoesNotContain(tsClasses, c => c.ClassName == "UnmarkedClass");
	}

	[Fact]
	public void TsClasses_BaseClassOfMarkedClass_IsIncludedAndFlaggedAsBaseType()
	{
		var source = """
			namespace Test;
			public class TypeScriptModelAttribute : System.Attribute { }
			public class BaseClass { }
			[TypeScriptModel]
			public class DerivedClass : BaseClass { }
			""";

		var tsClasses = RoslynTestHelper.BuildSolutionModel(source).TsClasses;

		var baseClass = tsClasses.FirstOrDefault(c => c.ClassName == "BaseClass");
		Assert.NotNull(baseClass);
		Assert.True(baseClass.IsBaseType);
	}

	[Fact]
	public void TsClasses_RegularClass_TsNameMatchesClassName()
	{
		var source = """
			namespace Test;
			public class TypeScriptModelAttribute : System.Attribute { }
			[TypeScriptModel]
			public class MyViewModel { }
			""";

		var c = RoslynTestHelper.BuildSolutionModel(source).TsClasses
			.Single(x => x.ClassName == "MyViewModel");

		Assert.Equal("MyViewModel", c.TsName);
	}

	[Fact]
	public void TsClasses_InterfaceClass_TsNameHasIPrefix()
	{
		var source = """
			namespace Test;
			public class TypeScriptModelAttribute : System.Attribute
			{
				public bool IsInterface { get; set; }
			}
			[TypeScriptModel(IsInterface = true)]
			public class ContactVM { }
			""";

		var c = RoslynTestHelper.BuildSolutionModel(source).TsClasses
			.Single(x => x.ClassName == "ContactVM");

		Assert.Equal("IContactVM", c.TsName);
	}
}

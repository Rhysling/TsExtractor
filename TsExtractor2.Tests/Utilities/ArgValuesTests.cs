using TsExtractor2.Utilities;

namespace TsExtractor2.Tests.Utilities;

// ArgValues is a static class backed by a static dictionary. Tests in this class run
// sequentially (xunit v2 default within a class). Each test calls LoadFromArgs with
// its own args, which replaces the dictionary entirely, so state does not leak between tests.

public class ArgValuesTests
{
	[Fact]
	public void LoadFromArgs_SourcePath_IsReadBack()
	{
		ArgValues.LoadFromArgs(["SourcePath=D:\\solution.sln"]);
		Assert.Equal("D:\\solution.sln", ArgValues.SourcePath);
	}

	[Fact]
	public void LoadFromArgs_OutPath_IsReadBack()
	{
		ArgValues.LoadFromArgs(["OutPath=D:\\out\\types.d.ts"]);
		Assert.Equal("D:\\out\\types.d.ts", ArgValues.OutPath);
	}

	[Fact]
	public void LoadFromArgs_ExcludeProjectNames_IsSplitOnComma()
	{
		ArgValues.LoadFromArgs(["ExcludeProjectNames=Alpha,Beta,Gamma"]);
		Assert.Equal(["Alpha", "Beta", "Gamma"], ArgValues.ExcludeProjectNames);
	}

	[Fact]
	public void LoadFromArgs_KeysAreCaseInsensitive()
	{
		ArgValues.LoadFromArgs(["SOURCEPATH=test.sln"]);
		Assert.Equal("test.sln", ArgValues.SourcePath);
	}

	[Fact]
	public void LoadFromArgs_AbsentKey_ReturnsNullOrEmptyArray()
	{
		ArgValues.LoadFromArgs(["SourcePath=test.sln"]);
		Assert.Null(ArgValues.OutPath);
		Assert.Empty(ArgValues.ExcludeProjectNames);
	}

	[Fact]
	public void LoadFromArgs_EntryWithNoEquals_IsIgnored()
	{
		ArgValues.LoadFromArgs(["SourcePath=test.sln", "NoEqualsHere"]);
		// The arg without '=' must not be parsed as a key — SourcePath should still be set.
		Assert.Equal("test.sln", ArgValues.SourcePath);
	}
}

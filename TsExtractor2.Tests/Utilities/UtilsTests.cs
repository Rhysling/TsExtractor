using TsExtractor2.Utilities;

namespace TsExtractor2.Tests.Utilities;

public class UtilsTests
{
	[Fact]
	public void FindSlnPath_ExceedsMaxDepth_ReturnsNull()
	{
		// iteration 6 → itCount 7 > 6 → immediate null without touching the file system.
		Assert.Null(Utils.FindSlnPath("C:\\AnyPath", 6));
	}

	[Fact]
	public void FindSlnPath_SlnInImmediateParent_ReturnsSlnPath()
	{
		// Layout: tempRoot/MySolution.sln  +  tempRoot/Sub/  (search starts in Sub)
		string tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
		string subDir = Path.Combine(tempRoot, "Sub");
		string slnPath = Path.Combine(tempRoot, "MySolution.sln");

		Directory.CreateDirectory(subDir);
		File.WriteAllText(slnPath, "");

		try
		{
			var result = Utils.FindSlnPath(subDir, 0);
			Assert.Equal(slnPath, result);
		}
		finally
		{
			Directory.Delete(tempRoot, recursive: true);
		}
	}
}

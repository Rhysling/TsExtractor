using TsExtractor2.Utilities;

namespace TsExtractor2.Tests.Utilities;

public class StringExtTests
{
	[Fact]
	public void CamelCase_PascalCaseInput_LowercasesFirstLetter()
	{
		Assert.Equal("myProperty", "MyProperty".CamelCase());
	}

	[Fact]
	public void CamelCase_UnderscorePrefix_ConvertsInnerPartOnly()
	{
		Assert.Equal("_myField", "_MyField".CamelCase());
	}

	[Fact]
	public void CamelCase_WhitespaceInput_ReturnsEmpty()
	{
		Assert.Equal("", "   ".CamelCase());
	}

	[Fact]
	public void ProperCase_LowercaseFirstLetter_UppercasesFirstLetter()
	{
		Assert.Equal("MyProperty", "myProperty".ProperCase());
	}
}

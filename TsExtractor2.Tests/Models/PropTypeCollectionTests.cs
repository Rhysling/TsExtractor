using TsExtractor2.Models;

namespace TsExtractor2.Tests.Models;

public class PropTypeCollectionTests
{
	[Fact]
	public void FlattenTypeNames_NestedNullableInt_ReturnsFlatListOuterFirst()
	{
		// Mirrors PropTypeCollection built for int? (Nullable<Int32>):
		// outer = "Nullable/", inner = "Int32"
		var ptc = new PropTypeCollection
		{
			TypeName = "Nullable/",
			SubSummary = new PropTypeCollection { TypeName = "Int32" }
		};

		var result = ptc.FlattenTypeNames();

		Assert.Equal(["Nullable/", "Int32"], result);
	}

	[Fact]
	public void FlattenTypeNames_SingleLeaf_ReturnsSingleItemList()
	{
		var ptc = new PropTypeCollection { TypeName = "String" };

		Assert.Equal(["String"], ptc.FlattenTypeNames());
	}

	[Fact]
	public void FlattenTypeNames_ThreeLevels_ReturnsAllInOrder()
	{
		// Mirrors List<int?> — outer "List/", middle "Nullable/", leaf "Int32"
		var ptc = new PropTypeCollection
		{
			TypeName = "List/",
			SubSummary = new PropTypeCollection
			{
				TypeName = "Nullable/",
				SubSummary = new PropTypeCollection { TypeName = "Int32" }
			}
		};

		Assert.Equal(["List/", "Nullable/", "Int32"], ptc.FlattenTypeNames());
	}
}

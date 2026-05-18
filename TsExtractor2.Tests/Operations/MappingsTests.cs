using TsExtractor2.Operations;

namespace TsExtractor2.Tests.Operations;

// MapPropTypeNamesToTsType receives the flattened type name list produced by
// PropTypeCollection.FlattenTypeNames(), where the outermost wrapper (e.g. "List/",
// "Nullable/") appears first and the leaf type appears last. The method reverses
// that list internally before processing, so tests must pass the list in that order.

public class MappingsTests
{
	private static readonly List<string> NoClasses = [];

	[Fact]
	public void MapPropTypeNames_PrimitiveInt_ReturnsNumber()
	{
		Assert.Equal("number", Mappings.MapPropTypeNamesToTsType(["Int32"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_NullableInt_ReturnsINullableNumber()
	{
		// "Nullable/" is the outer wrapper; "Int32" is the leaf — matching FlattenTypeNames output.
		Assert.Equal("INullable<number>", Mappings.MapPropTypeNamesToTsType(["Nullable/", "Int32"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_ListOfString_ReturnsArrayString()
	{
		Assert.Equal("Array<string>", Mappings.MapPropTypeNamesToTsType(["List/", "String"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_UnmappedType_ReturnsAny()
	{
		Assert.Equal("any", Mappings.MapPropTypeNamesToTsType(["SomeUnmappedType"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_Boolean_ReturnsBoolean()
	{
		Assert.Equal("boolean", Mappings.MapPropTypeNamesToTsType(["Boolean"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_Int64_ReturnsErrorString()
	{
		// long is intentionally unsupported; the error string makes the problem visible in TS tooling.
		Assert.Equal("error-long not supported in js", Mappings.MapPropTypeNamesToTsType(["Int64"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_DateTime_ReturnsString()
	{
		Assert.Equal("string", Mappings.MapPropTypeNamesToTsType(["DateTime"], NoClasses));
	}

	[Fact]
	public void MapPropTypeNames_KnownTsClass_ReturnsClassNameDirectly()
	{
		Assert.Equal("OrderVM", Mappings.MapPropTypeNamesToTsType(["OrderVM"], ["OrderVM", "IContactMessage"]));
	}

	[Fact]
	public void MapPropTypeNames_KnownTsInterface_ReturnsIPrefixedName()
	{
		// The leaf type is written without "I" in C# code; the mapping finds "I" + name in tsClassList.
		Assert.Equal("IContactMessage", Mappings.MapPropTypeNamesToTsType(["ContactMessage"], ["OrderVM", "IContactMessage"]));
	}

	[Fact]
	public void MapPropTypeNames_NestedListOfNullableInt_ReturnsWrappedType()
	{
		// FlattenTypeNames order for List<int?>:  ["List/", "Nullable/", "Int32"]
		Assert.Equal("Array<INullable<number>>", Mappings.MapPropTypeNamesToTsType(["List/", "Nullable/", "Int32"], NoClasses));
	}
}

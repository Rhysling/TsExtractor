using TsExtractor2.Models;

namespace TsExtractor2.Tests.Models;

public class ClassModelTests
{
	private static ClassModel Make(string? baseTypeName) => new()
	{
		FilePath = "test.cs",
		NamespaceName = "Test.Namespace",
		ClassName = "MyClass",
		BaseTypeName = baseTypeName
	};

	[Fact]
	public void HasBaseType_ConcreteBaseClass_ReturnsTrue()
	{
		Assert.True(Make("ViewModelBase").HasBaseType);
	}

	[Fact]
	public void HasBaseType_SystemOrNullBase_ReturnsFalse()
	{
		// "Object", "Record", and "Attribute" are excluded by HasBaseType; null is also false.
		Assert.False(Make(null).HasBaseType);
		Assert.False(Make("Object").HasBaseType);
		Assert.False(Make("Record").HasBaseType);
		Assert.False(Make("Attribute").HasBaseType);
	}
}

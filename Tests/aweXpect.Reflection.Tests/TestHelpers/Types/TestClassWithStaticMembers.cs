namespace aweXpect.Reflection.Tests.TestHelpers.Types;

public class TestClassWithStaticMembers
{
	// Static field
	public static string StaticField = "static";

	// Non-static field  
	public string NonStaticField = "non-static";

	// Static constructor
	static TestClassWithStaticMembers()
	{
		StaticProperty = "initialized";
	}

	// Non-static constructor
	public TestClassWithStaticMembers()
	{
		NonStaticProperty = "initialized";
	}

	// Static property
	public static string StaticProperty { get; set; }

	// Non-static property
	public string NonStaticProperty { get; set; }

	// Static method
	public static void StaticMethod()
	{
	}

	// Non-static method
	public void NonStaticMethod()
	{
	}
}

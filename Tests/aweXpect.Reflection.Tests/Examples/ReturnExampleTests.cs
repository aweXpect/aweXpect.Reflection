using System.Threading.Tasks;

namespace aweXpect.Reflection.Tests.Examples;

/// <summary>
/// Example test demonstrating the new Return expectation functionality
/// </summary>
public sealed class ReturnExampleTests
{
#if NET8_0_OR_GREATER
	[Fact]
	public async Task AllAsyncMethodsReturnTask()
	{
		// This demonstrates the basic Return<T>() functionality requested in issue #70
		await That(In.AssemblyContaining(typeof(In))
				.Methods().WithName("Async").AsSuffix())
			.Return<Task>();
	}
#endif

	[Fact]  
	public async Task AllGetMethodsReturnSpecificTypes()
	{
		// This demonstrates both generic and non-generic Return functionality
		await That(In.Type<TestClass>().Methods().WithName("GetString"))
			.Return<string>();

		await That(In.Type<TestClass>().Methods().WithName("GetInt"))
			.Return(typeof(int));
	}

	[Fact]
	public async Task ReturnWithInheritanceSupport()
	{
		// This demonstrates that inheritance is supported
		await That(In.Type<TestClass>().Methods().WithName("GetDerived"))
			.Return<BaseClass>();
	}

	[Fact]
	public async Task SingleMethodReturnsExpectation()
	{
		// This demonstrates using Returns for single methods (grammatically correct)
		MethodInfo method = typeof(TestClass).GetMethod("GetString")!;
		
		await That(method).Returns<string>();
	}

	[Fact]
	public async Task OrReturnChaining()
	{
		// This demonstrates chaining multiple return types for collections
		await That(In.Type<TestClass>().Methods().WithName("GetString"))
			.Return<int>().OrReturn<string>().OrReturn<object>();
	}

	[Fact]
	public async Task OrReturnsChainingSingleMethod()
	{
		// This demonstrates chaining multiple return types for single method
		MethodInfo method = typeof(TestClass).GetMethod("GetString")!;
		
		await That(method).Returns<int>().OrReturns<string>().OrReturns<object>();
	}

#pragma warning disable CA1822 // Mark members as static
	private class TestClass
	{
		public string GetString() => "test";
		public int GetInt() => 42;
		public DerivedClass GetDerived() => new();
	}
#pragma warning restore CA1822

	private class BaseClass { }
	private class DerivedClass : BaseClass { }
}
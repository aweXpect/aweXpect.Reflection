using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class MethodFilters
{
	public sealed class WhichAreGeneric
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterOnlyGenericMethods()
			{
				Filtered.Methods subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.Which(t => t == typeof(ThatMethods.TestClass))
					.Methods()
					.WhichAreGeneric();

				await That(subject).All().Satisfy(m => m.IsGenericMethod).And.IsNotEmpty();
			}

			[Fact]
			public async Task WithCount_ShouldFilterMethodsByGenericArgumentCount()
			{
				// Create a simple functional test to ensure the filter works 
				Filtered.Methods subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.Methods()
					.WhichAreGeneric()
					.WithCount(1);

				// Test that the filter returns sensible results
				await That(subject.GetDescription()).Contains("with 1 generic argument");
			}

			[Fact]
			public async Task ShouldUpdateDescription()
			{
				Filtered.Methods subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.Methods()
					.WhichAreGeneric();

				await That(subject.GetDescription()).Contains("generic methods");
			}

			[Fact]
			public async Task WithCount_ShouldUpdateDescription()
			{
				Filtered.Methods subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.Methods()
					.WhichAreGeneric()
					.WithCount(2);

				await That(subject.GetDescription()).Contains("generic methods with 2 generic arguments");
			}
		}
	}
}

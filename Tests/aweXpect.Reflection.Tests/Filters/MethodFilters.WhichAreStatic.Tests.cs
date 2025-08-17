using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class MethodFilters
{
	public sealed class WhichAreStatic
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForStaticMethods()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WhichAreStatic();

				await That(methods).All().Satisfy(m => m.IsStatic);
				await That(methods.GetDescription())
					.IsEqualTo("static methods in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForNonStaticMethods()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WhichAreNotStatic();

				await That(methods).All().Satisfy(m => !m.IsStatic);
				await That(methods.GetDescription())
					.IsEqualTo("non-static methods in assembly").AsPrefix();
			}
		}
	}
}
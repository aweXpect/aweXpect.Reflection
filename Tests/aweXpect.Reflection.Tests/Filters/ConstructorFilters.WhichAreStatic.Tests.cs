using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ConstructorFilters
{
	public sealed class WhichAreStatic
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForStaticConstructors()
			{
				// Note: Static constructors exist but are rare
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAreStatic();

				await That(constructors).All().Satisfy(c => c.IsStatic);
				await That(constructors.GetDescription())
					.IsEqualTo("static constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForNonStaticConstructors()
			{
				// Most constructors are non-static
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAreNotStatic();

				await That(constructors).All().Satisfy(c => !c.IsStatic);
				await That(constructors.GetDescription())
					.IsEqualTo("non-static constructors in assembly").AsPrefix();
			}
		}
	}
}
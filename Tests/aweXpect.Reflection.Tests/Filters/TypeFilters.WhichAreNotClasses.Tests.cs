using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreNotClasses
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForNonClassesWithExplicitMethod()
			{
				Filtered.Types types = In.AssemblyContaining<AssemblyFilters>()
					.Types().WhichAreNotClasses();

				await That(types).AreNotClasses();
				await That(types.GetDescription())
					.IsEqualTo("which are not classes types in assembly").AsPrefix();
			}
		}
	}
}
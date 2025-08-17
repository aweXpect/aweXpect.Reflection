using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreClasses
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForClassesWithExplicitMethod()
			{
				Filtered.Types types = In.AssemblyContaining<AssemblyFilters>()
					.Types().WhichAreClasses();

				await That(types).AreClasses();
				await That(types.GetDescription())
					.IsEqualTo("which are classes types in assembly").AsPrefix();
			}
		}
	}
}
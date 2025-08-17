using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreEnums
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForEnumsWithExplicitMethod()
			{
				Filtered.Types types = In.AssemblyContaining<AssemblyFilters>()
					.Types().WhichAreEnums();

				await That(types).AreEnums();
				await That(types.GetDescription())
					.IsEqualTo("which are enums types in assembly").AsPrefix();
			}
		}
	}
}
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreNotEnums
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForNonEnumsWithExplicitMethod()
			{
				Filtered.Types types = In.AssemblyContaining<AssemblyFilters>()
					.Types().WhichAreNotEnums();

				await That(types).AreNotEnums();
				await That(types.GetDescription())
					.IsEqualTo("which are not enums types in assembly").AsPrefix();
			}
		}
	}
}
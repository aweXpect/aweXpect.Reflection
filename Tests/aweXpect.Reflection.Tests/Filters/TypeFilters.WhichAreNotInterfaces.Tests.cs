using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreNotInterfaces
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForNonInterfacesWithExplicitMethod()
			{
				Filtered.Types types = In.AssemblyContaining<AssemblyFilters>()
					.Types().WhichAreNotInterfaces();

				await That(types).AreNotInterfaces();
				await That(types.GetDescription())
					.IsEqualTo("which are not interfaces types in assembly").AsPrefix();
			}
		}
	}
}
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreInterfaces
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInterfacesWithExplicitMethod()
			{
				Filtered.Types types = In.AssemblyContaining<AssemblyFilters>()
					.Types().WhichAreInterfaces();

				await That(types).AreInterfaces();
				await That(types.GetDescription())
					.IsEqualTo("which are interfaces types in assembly").AsPrefix();
			}
		}
	}
}
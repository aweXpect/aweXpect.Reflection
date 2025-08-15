using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class PropertyFilters
{
	public sealed class WhichArePublic
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForPublicPropertysWithExplicitMethod()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichArePublic();

				await That(properties).ArePublic();
				await That(properties.GetDescription())
					.IsEqualTo("public properties in assembly").AsPrefix();
			}
		}
	}
}

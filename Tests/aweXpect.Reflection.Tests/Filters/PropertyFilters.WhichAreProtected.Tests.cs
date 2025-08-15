using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class PropertyFilters
{
	public sealed class WhichAreProtected
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForProtectedPropertysWithExplicitMethod()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreProtected();

				await That(properties).AreProtected();
				await That(properties.GetDescription())
					.IsEqualTo("protected properties in assembly").AsPrefix();
			}
		}
	}
}

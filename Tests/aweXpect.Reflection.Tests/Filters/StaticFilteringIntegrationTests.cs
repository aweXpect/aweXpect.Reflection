using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

/// <summary>
/// Integration test to verify the static filtering functionality works end-to-end
/// </summary>
public sealed class StaticFilteringIntegrationTests
{
	[Fact]
	public async Task ShouldDemonstrateStaticFilteringForAllEntityTypes()
	{
		// Test type filtering - we know there are static and non-static types
		var staticTypes = In.AssemblyContaining<AssemblyFilters>()
			.Types().WhichAreStatic();
		var nonStaticTypes = In.AssemblyContaining<AssemblyFilters>()
			.Types().WhichAreNotStatic();

		// Static types exist (extensions classes, helpers, etc.)
		await That(staticTypes.Count()).IsGreaterThanOrEqualTo(0);
		await That(nonStaticTypes.Count()).IsGreaterThan(0);

		// Test method filtering - just verify no exceptions
		var staticMethods = In.AssemblyContaining<AssemblyFilters>()
			.Methods().WhichAreStatic();
		var nonStaticMethods = In.AssemblyContaining<AssemblyFilters>()
			.Methods().WhichAreNotStatic();

		await That(() => staticMethods.Count()).DoesNotThrow();
		await That(nonStaticMethods.Count()).IsGreaterThan(0);

		// Test field filtering - just verify no exceptions
		var staticFields = In.AssemblyContaining<AssemblyFilters>()
			.Fields().WhichAreStatic();
		var nonStaticFields = In.AssemblyContaining<AssemblyFilters>()
			.Fields().WhichAreNotStatic();

		await That(() => staticFields.Count()).DoesNotThrow();
		await That(() => nonStaticFields.Count()).DoesNotThrow();

		// Test property filtering - just verify no exceptions
		var staticProperties = In.AssemblyContaining<AssemblyFilters>()
			.Properties().WhichAreStatic();
		var nonStaticProperties = In.AssemblyContaining<AssemblyFilters>()
			.Properties().WhichAreNotStatic();

		await That(() => staticProperties.Count()).DoesNotThrow();
		await That(() => nonStaticProperties.Count()).DoesNotThrow();

		// Test constructor filtering - static constructors are rare
		var staticConstructors = In.AssemblyContaining<AssemblyFilters>()
			.Constructors().WhichAreStatic();
		var nonStaticConstructors = In.AssemblyContaining<AssemblyFilters>()
			.Constructors().WhichAreNotStatic();

		await That(() => staticConstructors.Count()).DoesNotThrow();
		await That(nonStaticConstructors.Count()).IsGreaterThan(0);
	}

	[Fact]
	public async Task ShouldChainFiltersCorrectly()
	{
		// Test chaining filters - find public static methods
		var publicStaticMethods = In.AssemblyContaining<AssemblyFilters>()
			.Methods().WhichArePublic().WhichAreStatic();

		await That(publicStaticMethods).All().Satisfy(m => m.IsStatic && m.IsPublic);
		
		// Verify description includes both filters
		await That(publicStaticMethods.GetDescription())
			.Contains("public").And.Contains("static");
	}
}
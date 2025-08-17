using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class PropertyFilters
	{
		public sealed class WhichAreSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForSealedProperties()
				{
					Filtered.Properties properties = In.AssemblyContaining<PropertyTestClasses.SealedPropertyClass>()
						.Types().Properties().WhichAreSealed();

					await That(properties.Count()).IsGreaterThan(0);
					await That(properties.GetDescription())
						.IsEqualTo("sealed properties in types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonSealedProperties()
				{
					Filtered.Properties properties = In.AssemblyContaining<PropertyTestClasses.ConcretePropertyClass>()
						.Types().Properties().WhichAreNotSealed();

					await That(properties.Count()).IsGreaterThan(0);
					await That(properties.GetDescription())
						.IsEqualTo("non-sealed properties in types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.PropertyTestClasses
{
	public class SealedPropertyClass : AbstractPropertyClass
	{
		public sealed override string AbstractProperty { get; set; } = "";
		public sealed override string VirtualProperty { get; set; } = "";
	}
}
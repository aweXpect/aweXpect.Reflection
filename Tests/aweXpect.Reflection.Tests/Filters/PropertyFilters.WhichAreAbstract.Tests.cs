using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class PropertyFilters
	{
		public sealed class WhichAreAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForAbstractProperties()
				{
					Filtered.Properties properties = In.AssemblyContaining<PropertyTestClasses.AbstractPropertyClass>()
						.Types().Properties().WhichAreAbstract();

					await That(properties.Count()).IsGreaterThan(0);
					await That(properties.GetDescription())
						.IsEqualTo("abstract properties in types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonAbstractProperties()
				{
					Filtered.Properties properties = In.AssemblyContaining<PropertyTestClasses.ConcretePropertyClass>()
						.Types().Properties().WhichAreNotAbstract();

					await That(properties.Count()).IsGreaterThan(0);
					await That(properties.GetDescription())
						.IsEqualTo("non-abstract properties in types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.PropertyTestClasses
{
	public abstract class AbstractPropertyClass
	{
		public abstract string AbstractProperty { get; set; }
		public virtual string VirtualProperty { get; set; } = "";
	}

	public class ConcretePropertyClass
	{
		public virtual string VirtualProperty { get; set; } = "";
		public string ConcreteProperty { get; set; } = "";
	}
}
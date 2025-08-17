using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class MethodFilters
	{
		public sealed class WhichAreAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForAbstractMethods()
				{
					Filtered.Methods methods = In.AssemblyContaining<MethodTestClasses.AbstractMethodClass>()
						.Types().Methods().WhichAreAbstract();

					await That(methods.Count()).IsGreaterThan(0);
					await That(methods.GetDescription())
						.IsEqualTo("abstract methods in types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonAbstractMethods()
				{
					Filtered.Methods methods = In.AssemblyContaining<MethodTestClasses.ConcreteMethodClass>()
						.Types().Methods().WhichAreNotAbstract();

					await That(methods.Count()).IsGreaterThan(0);
					await That(methods.GetDescription())
						.IsEqualTo("non-abstract methods in types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.MethodTestClasses
{
	public abstract class AbstractMethodClass
	{
		public abstract void AbstractMethod();
		public virtual void VirtualMethod() { }
	}

	public class ConcreteMethodClass
	{
		public virtual void VirtualMethod() { }
		public void ConcreteMethod() { }
	}
}
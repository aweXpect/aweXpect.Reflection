using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class MethodFilters
	{
		public sealed class WhichAreSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForSealedMethods()
				{
					Filtered.Methods methods = In.AssemblyContaining<MethodTestClasses.SealedMethodClass>()
						.Types().Methods().WhichAreSealed();

					await That(methods.Count()).IsGreaterThan(0);
					await That(methods.GetDescription())
						.IsEqualTo("sealed methods in types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonSealedMethods()
				{
					Filtered.Methods methods = In.AssemblyContaining<MethodTestClasses.ConcreteMethodClass>()
						.Types().Methods().WhichAreNotSealed();

					await That(methods.Count()).IsGreaterThan(0);
					await That(methods.GetDescription())
						.IsEqualTo("non-sealed methods in types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.MethodTestClasses
{
	public class SealedMethodClass : ConcreteMethodClass
	{
		public sealed override string ToString() => "SealedMethodClass";
	}
}
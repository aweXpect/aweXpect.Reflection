using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class TypeFilters
	{
		public sealed class WhichAreSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForSealedTypes()
				{
					Filtered.Types types = In.AssemblyContaining<TestClasses.SealedTestClass>()
						.Types().WhichAreSealed();

					await That(types).AreSealed();
					await That(types.GetDescription())
						.IsEqualTo("sealed types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonSealedTypes()
				{
					Filtered.Types types = In.AssemblyContaining<TestClasses.ConcreteTestClass>()
						.Types().WhichAreNotSealed();

					await That(types).AreNotSealed();
					await That(types.GetDescription())
						.IsEqualTo("non-sealed types in assembly").AsPrefix();
				}
			}
		}
	}
}
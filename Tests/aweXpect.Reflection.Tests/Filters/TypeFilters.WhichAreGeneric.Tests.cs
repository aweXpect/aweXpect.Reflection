using System.Collections.Generic;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	public sealed class WhichAreGeneric
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterOnlyGenericTypes()
			{
				Filtered.Types subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.WhichAreGeneric();

				await That(subject).AreGeneric().And.IsNotEmpty();
			}

			[Fact]
			public async Task WithCount_ShouldFilterTypesByGenericArgumentCount()
			{
				// Test that the filter returns sensible results by checking description
				Filtered.Types subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.WhichAreGeneric()
					.WithCount(1);

				await That(subject.GetDescription()).Contains("with 1 generic argument");
			}

			[Fact]
			public async Task ShouldUpdateDescription()
			{
				Filtered.Types subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.WhichAreGeneric();

				await That(subject.GetDescription()).Contains("generic types");
			}

			[Fact]
			public async Task WithCount_ShouldUpdateDescription()
			{
				Filtered.Types subject = In.AssemblyContaining<WhichAreGeneric>().Types()
					.WhichAreGeneric()
					.WithCount(1);

				await That(subject.GetDescription()).Contains("generic types with 1 generic argument");
			}
		}
	}
}

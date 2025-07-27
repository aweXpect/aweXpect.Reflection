using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class FieldFilters
{
	public sealed class WhichAreNot
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAreNot(AccessModifiers.Internal);

				await That(types).AreNotInternal();
				await That(types.GetDescription())
					.IsEqualTo("non-internal fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAreNot(AccessModifiers.Private);

				await That(types).AreNotPrivate();
				await That(types.GetDescription())
					.IsEqualTo("non-private fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAreNot(AccessModifiers.Protected);

				await That(types).AreNotProtected();
				await That(types.GetDescription())
					.IsEqualTo("non-protected fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAreNot(AccessModifiers.Public);

				await That(types).AreNotPublic();
				await That(types.GetDescription())
					.IsEqualTo("non-public fields in assembly").AsPrefix();
			}
		}
	}
}

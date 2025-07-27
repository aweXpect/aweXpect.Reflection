using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class PropertyFilters
{
	public sealed class WhichAreNot
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreNot(AccessModifiers.Internal);

				await That(types).AreNotInternal();
				await That(types.GetDescription())
					.IsEqualTo("non-internal properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreNot(AccessModifiers.Private);

				await That(types).AreNotPrivate();
				await That(types.GetDescription())
					.IsEqualTo("non-private properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreNot(AccessModifiers.Protected);

				await That(types).AreNotProtected();
				await That(types.GetDescription())
					.IsEqualTo("non-protected properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreNot(AccessModifiers.Public);

				await That(types).AreNotPublic();
				await That(types.GetDescription())
					.IsEqualTo("non-public properties in assembly").AsPrefix();
			}
		}
	}
}

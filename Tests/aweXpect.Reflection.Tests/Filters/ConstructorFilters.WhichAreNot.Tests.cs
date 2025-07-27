using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ConstructorFilters
{
	public sealed class WhichAreNot
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAreNot(AccessModifiers.Internal);

				await That(types).AreNotInternal();
				await That(types.GetDescription())
					.IsEqualTo("non-internal constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAreNot(AccessModifiers.Private);

				await That(types).AreNotPrivate();
				await That(types.GetDescription())
					.IsEqualTo("non-private constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAreNot(AccessModifiers.Protected);

				await That(types).AreNotProtected();
				await That(types.GetDescription())
					.IsEqualTo("non-protected constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAreNot(AccessModifiers.Public);

				await That(types).AreNotPublic();
				await That(types.GetDescription())
					.IsEqualTo("non-public constructors in assembly").AsPrefix();
			}
		}
	}
}

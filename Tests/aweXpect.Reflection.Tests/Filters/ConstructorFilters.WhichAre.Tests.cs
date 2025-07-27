using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ConstructorFilters
{
	public sealed class WhichAre
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAre(AccessModifiers.Internal);

				await That(types).AreInternal();
				await That(types.GetDescription())
					.IsEqualTo("internal constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAre(AccessModifiers.Private);

				await That(types).ArePrivate();
				await That(types.GetDescription())
					.IsEqualTo("private constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAre(AccessModifiers.Protected);

				await That(types).AreProtected();
				await That(types.GetDescription())
					.IsEqualTo("protected constructors in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Constructors types = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WhichAre(AccessModifiers.Public);

				await That(types).ArePublic();
				await That(types.GetDescription())
					.IsEqualTo("public constructors in assembly").AsPrefix();
			}
		}
	}
}

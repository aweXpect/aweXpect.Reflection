using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class FieldFilters
{
	public sealed class WhichAre
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAre(AccessModifiers.Internal);

				await That(types).AreInternal();
				await That(types.GetDescription())
					.IsEqualTo("internal fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAre(AccessModifiers.Private);

				await That(types).ArePrivate();
				await That(types.GetDescription())
					.IsEqualTo("private fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAre(AccessModifiers.Protected);

				await That(types).AreProtected();
				await That(types.GetDescription())
					.IsEqualTo("protected fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Fields types = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAre(AccessModifiers.Public);

				await That(types).ArePublic();
				await That(types.GetDescription())
					.IsEqualTo("public fields in assembly").AsPrefix();
			}
		}
	}
}

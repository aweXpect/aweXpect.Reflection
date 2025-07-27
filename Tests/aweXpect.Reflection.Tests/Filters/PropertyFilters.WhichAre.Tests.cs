using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class PropertyFilters
{
	public sealed class WhichAre
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Internal);

				await That(types).AreInternal();
				await That(types.GetDescription())
					.IsEqualTo("internal properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Private);

				await That(types).ArePrivate();
				await That(types.GetDescription())
					.IsEqualTo("private properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Protected);

				await That(types).AreProtected();
				await That(types.GetDescription())
					.IsEqualTo("protected properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Properties types = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Public);

				await That(types).ArePublic();
				await That(types.GetDescription())
					.IsEqualTo("public properties in assembly").AsPrefix();
			}
		}
	}
}

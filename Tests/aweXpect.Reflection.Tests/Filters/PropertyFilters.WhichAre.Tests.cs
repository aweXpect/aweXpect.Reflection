using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class PropertyFilters
{
	public sealed class WhichAre
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalProperties()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Internal);

				await That(properties).AreInternal();
				await That(properties.GetDescription())
					.IsEqualTo("internal properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateProperties()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Private);

				await That(properties).ArePrivate();
				await That(properties.GetDescription())
					.IsEqualTo("private properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedProperties()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Protected);

				await That(properties).AreProtected();
				await That(properties.GetDescription())
					.IsEqualTo("protected properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicProperties()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAre(AccessModifiers.Public);

				await That(properties).ArePublic();
				await That(properties.GetDescription())
					.IsEqualTo("public properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicPropertiesWithExplicitMethod()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichArePublic();

				await That(properties).ArePublic();
				await That(properties.GetDescription())
					.IsEqualTo("public properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivatePropertiesWithExplicitMethod()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichArePrivate();

				await That(properties).ArePrivate();
				await That(properties.GetDescription())
					.IsEqualTo("private properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedPropertiesWithExplicitMethod()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreProtected();

				await That(properties).AreProtected();
				await That(properties.GetDescription())
					.IsEqualTo("protected properties in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForInternalPropertiesWithExplicitMethod()
			{
				Filtered.Properties properties = In.AssemblyContaining<AssemblyFilters>()
					.Properties().WhichAreInternal();

				await That(properties).AreInternal();
				await That(properties.GetDescription())
					.IsEqualTo("internal properties in assembly").AsPrefix();
			}
		}
	}
}

using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class EventFilters
{
	public sealed class WhichAre
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForInternalTypes()
			{
				Filtered.Events types = In.AssemblyContaining<AssemblyFilters>()
					.Events().WhichAre(AccessModifiers.Internal);

				await That(types).AreInternal();
				await That(types.GetDescription())
					.IsEqualTo("internal events in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPrivateTypes()
			{
				Filtered.Events types = In.AssemblyContaining<AssemblyFilters>()
					.Events().WhichAre(AccessModifiers.Private);

				await That(types).ArePrivate();
				await That(types.GetDescription())
					.IsEqualTo("private events in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForProtectedTypes()
			{
				Filtered.Events types = In.AssemblyContaining<AssemblyFilters>()
					.Events().WhichAre(AccessModifiers.Protected);

				await That(types).AreProtected();
				await That(types.GetDescription())
					.IsEqualTo("protected events in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForPublicTypes()
			{
				Filtered.Events types = In.AssemblyContaining<AssemblyFilters>()
					.Events().WhichAre(AccessModifiers.Public);

				await That(types).ArePublic();
				await That(types.GetDescription())
					.IsEqualTo("public events in assembly").AsPrefix();
			}
		}
	}
}

using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class FieldFilters
{
	public sealed class WhichAreStatic
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldAllowFilteringForStaticFields()
			{
				Filtered.Fields fields = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAreStatic();

				await That(fields).All().Satisfy(f => f.IsStatic);
				await That(fields.GetDescription())
					.IsEqualTo("static fields in assembly").AsPrefix();
			}

			[Fact]
			public async Task ShouldAllowFilteringForNonStaticFields()
			{
				Filtered.Fields fields = In.AssemblyContaining<AssemblyFilters>()
					.Fields().WhichAreNotStatic();

				await That(fields).All().Satisfy(f => !f.IsStatic);
				await That(fields.GetDescription())
					.IsEqualTo("non-static fields in assembly").AsPrefix();
			}
		}
	}
}
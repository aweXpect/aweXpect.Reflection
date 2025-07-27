using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class MethodFilters
{
	public sealed class WhichSatisfy
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterForMethodsWhichSatisfyThePredicate()
			{
				Filtered.Methods types = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WhichSatisfy(it => it.Name.Equals("SomeMethodToVerifyTheNameOfIt"));

				await That(types).HasSingle().Which.IsEqualTo(ExpectedMethodInfo());
				await That(types.GetDescription())
					.IsEqualTo(
						"methods matching it => it.Name.Equals(\"SomeMethodToVerifyTheNameOfIt\") in assembly")
					.AsPrefix();
			}
		}
	}
}

using System.Linq;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatTypes
{
	public sealed class AreAbstract
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssembliesContainNonAbstractTypes_ShouldFail()
			{
				async Task Act()
					=> await That(In.AssemblyContaining<AreAbstract>().Types()).AreAbstract();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that In.AssemblyContaining<AreAbstract>().Types()
					             are all abstract,
					             but it contained non-abstract types [
					               *
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenFilteringOnlyAbstractTypes_ShouldSucceed()
			{
				async Task Act()
					=> await That(In.AssemblyContaining<AreAbstract>().Types().WhichAreAbstract())
						.AreAbstract();

				await That(Act).DoesNotThrow();
			}
		}
	}
}

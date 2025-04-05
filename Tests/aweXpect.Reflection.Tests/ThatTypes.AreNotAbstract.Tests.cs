using System.Linq;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatTypes
{
	public sealed class AreNotAbstract
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssembliesContainNonAbstractTypes_ShouldSucceed()
			{
				async Task Act()
					=> await That(In.AssemblyContaining<AreAbstract>().Types()).AreNotAbstract();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenFilteringOnlyAbstractTypes_ShouldFail()
			{
				async Task Act()
					=> await That(In.AssemblyContaining<AreAbstract>().Types().Where(type => type.IsAbstract))
						.AreNotAbstract();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that In.AssemblyContaining<AreAbstract>().Types().Where(type => type.IsAbstract)
					             are not all abstract,
					             but it only contained abstract types [
					               *
					             ]
					             """).AsWildcard();
			}
		}
	}
}

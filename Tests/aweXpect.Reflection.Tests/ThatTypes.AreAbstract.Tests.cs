using aweXpect.Reflection.Collections;
using Xunit.Sdk;

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
				Filtered.Types subject = In.AssemblyContaining<AreAbstract>().Types();

				async Task Act()
					=> await That(subject).AreAbstract();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that types in assembly containing type ThatTypes.AreAbstract
					             are all abstract,
					             but it contained non-abstract types [
					               *
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenFilteringOnlyAbstractTypes_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<AreAbstract>().Types()
					.WhichSatisfy(type => type is { IsAbstract: true, IsSealed: false, IsInterface: false, });

				async Task Act()
					=> await That(subject).AreAbstract();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenAssembliesContainAbstractTypes_ShouldFail()
			{
				Filtered.Types subject = In.AssemblyContaining<AreAbstract>().Types()
					.WhichSatisfy(type => type is { IsAbstract: true, IsSealed: false, IsInterface: false, });

				async Task Act()
					=> await That(subject).DoesNotComplyWith(they => they.AreAbstract());

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that *
					             are not all abstract,
					             but it only contained abstract types [
					               *
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenFilteringOnlyNonAbstractTypes_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<AreAbstract>().Sealed.Types();

				async Task Act()
					=> await That(subject).DoesNotComplyWith(they => they.AreAbstract());

				await That(Act).DoesNotThrow();
			}
		}
	}
}

using aweXpect.Reflection.Collections;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatTypes
{
	public sealed class DoNotInheritFrom
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAllTypesInherit_ShouldFail()
			{
				Filtered.Types subject = In.Types(typeof(DerivedClass1), typeof(DerivedClass2));

				async Task Act()
					=> await That(subject).DoNotInheritFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that in types [ThatTypes.DerivedClass1, ThatTypes.DerivedClass2]
					             not all inherit from ThatTypes.BaseClass,
					             but it only contained types that inherit from ThatTypes.BaseClass [
					               ThatTypes.DerivedClass1,
					               ThatTypes.DerivedClass2
					             ]
					             """);
			}

			[Fact]
			public async Task WhenNotAllTypesInherit_ShouldSucceed()
			{
				Filtered.Types subject = In.Types(typeof(DerivedClass1), typeof(UnrelatedClass));

				async Task Act()
					=> await That(subject).DoNotInheritFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}
		}
	}
}

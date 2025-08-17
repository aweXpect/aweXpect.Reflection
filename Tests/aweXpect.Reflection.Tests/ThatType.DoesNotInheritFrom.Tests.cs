using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class DoesNotInheritFrom
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenTypeDoesNotInherit_ShouldSucceed()
			{
				Type subject = typeof(UnrelatedClass);

				async Task Act()
					=> await That(subject).DoesNotInheritFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeInherits_ShouldFail()
			{
				Type subject = typeof(DerivedClass);

				async Task Act()
					=> await That(subject).DoesNotInheritFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             does not inherit from ThatType.BaseClass,
					             but it did inherit from ThatType.BaseClass
					             """);
			}
		}
	}
}

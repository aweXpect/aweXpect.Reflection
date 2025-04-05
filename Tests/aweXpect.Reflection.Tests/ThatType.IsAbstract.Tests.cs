namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsAbstract
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenTypeIsNotAbstract_ShouldFail()
			{
				Type subject = typeof(MyInstanceType);

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is abstract,
					             but it was non-abstract MyInstanceType
					             """);
			}

			[Fact]
			public async Task WhenTypeIsAbstract_ShouldSucceed()
			{
				Type subject = typeof(MyAbstractType);

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).DoesNotThrow();
			}
		}

		private abstract class MyAbstractType;

		private class MyInstanceType;
	}
}

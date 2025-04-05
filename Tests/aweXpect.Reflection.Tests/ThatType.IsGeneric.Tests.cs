namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsGeneric
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenTypeIsGeneric_ShouldSucceed()
			{
				Type subject = typeof(MyGenericType<int>);

				async Task Act()
					=> await That(subject).IsGeneric();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeIsNotGeneric_ShouldFail()
			{
				Type subject = typeof(MyInstanceType);

				async Task Act()
					=> await That(subject).IsGeneric();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is generic,
					             but it was non-generic MyInstanceType
					             """);
			}
		}

		private class MyGenericType<T>;

		private class MyInstanceType;
	}
}

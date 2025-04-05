﻿namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsNested
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenTypeIsNotNested_ShouldFail()
			{
				Type subject = typeof(ThatType);

				async Task Act()
					=> await That(subject).IsNested();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is nested,
					             but it was non-nested ThatType
					             """);
			}

			[Fact]
			public async Task WhenTypeIsNested_ShouldSucceed()
			{
				Type subject = typeof(IsNested);

				async Task Act()
					=> await That(subject).IsNested();

				await That(Act).DoesNotThrow();
			}
		}
	}
}

using System.Reflection;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class IsGeneric
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenMethodIsGeneric_ShouldSucceed()
			{
				MethodInfo? subject = GetMethod("GenericMethod");

				async Task Act()
					=> await That(subject).IsGeneric();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenMethodIsNotGeneric_ShouldFail()
			{
				MethodInfo? subject = GetMethod("NonGenericMethod");

				async Task Act()
					=> await That(subject).IsGeneric();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is generic,
					             but it was non-generic Int32 NonGenericMethod()
					             """);
			}

			[Fact]
			public async Task WhenMethodIsNull_ShouldFail()
			{
				MethodInfo? subject = null;

				async Task Act()
					=> await That(subject).IsGeneric();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is generic,
					             but it was <null>
					             """);
			}
		}
	}
}
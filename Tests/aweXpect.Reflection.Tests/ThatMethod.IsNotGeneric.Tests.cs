using System.Reflection;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class IsNotGeneric
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenMethodIsGeneric_ShouldFail()
			{
				MethodInfo? subject = GetMethod("GenericMethod");

				async Task Act()
					=> await That(subject).IsNotGeneric();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is not generic,
					             but it was generic T GenericMethod[T](T)
					             """);
			}

			[Fact]
			public async Task WhenMethodIsNotGeneric_ShouldSucceed()
			{
				MethodInfo? subject = GetMethod("NonGenericMethod");

				async Task Act()
					=> await That(subject).IsNotGeneric();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenMethodIsNull_ShouldFail()
			{
				MethodInfo? subject = null;

				async Task Act()
					=> await That(subject).IsNotGeneric();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is not generic,
					             but it was <null>
					             """);
			}
		}
	}
}
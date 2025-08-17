using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class IsAbstract
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenMethodIsAbstract_ShouldSucceed()
			{
				MethodInfo subject = typeof(AbstractClassWithMembers).GetMethod(nameof(AbstractClassWithMembers.AbstractMethod))!;

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenMethodIsNotAbstract_ShouldFail()
			{
				MethodInfo subject = typeof(AbstractClassWithMembers).GetMethod(nameof(AbstractClassWithMembers.VirtualMethod))!;

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).ThrowsException()
					.WithMessage($"""
					              Expected that subject
					              is abstract,
					              but it was non-abstract {Formatter.Format(subject)}
					              """);
			}

			[Fact]
			public async Task WhenMethodIsNull_ShouldFail()
			{
				MethodInfo? subject = null;

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is abstract,
					             but it was <null>
					             """);
			}
		}
	}
}
using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class IsSealed
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenMethodIsSealed_ShouldSucceed()
			{
				MethodInfo subject = typeof(ClassWithSealedMembers).GetMethod(nameof(ClassWithSealedMembers.VirtualMethod))!;

				async Task Act()
					=> await That(subject).IsSealed();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenMethodIsNotSealed_ShouldFail()
			{
				MethodInfo subject = typeof(AbstractClassWithMembers).GetMethod(nameof(AbstractClassWithMembers.VirtualMethod))!;

				async Task Act()
					=> await That(subject).IsSealed();

				await That(Act).ThrowsException()
					.WithMessage($"""
					              Expected that subject
					              is sealed,
					              but it was non-sealed {Formatter.Format(subject)}
					              """);
			}

			[Fact]
			public async Task WhenMethodIsNull_ShouldFail()
			{
				MethodInfo? subject = null;

				async Task Act()
					=> await That(subject).IsSealed();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is sealed,
					             but it was <null>
					             """);
			}
		}
	}
}
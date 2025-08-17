using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatField
{
	public sealed class IsNotStatic
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenFieldIsNotStatic_ShouldSucceed()
			{
				FieldInfo subject = typeof(TestClassWithStaticMembers).GetField(nameof(TestClassWithStaticMembers.NonStaticField))!;

				async Task Act()
					=> await That(subject).IsNotStatic();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenFieldIsNull_ShouldFail()
			{
				FieldInfo? subject = null;

				async Task Act()
					=> await That(subject).IsNotStatic();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is not static,
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenFieldIsStatic_ShouldFail()
			{
				FieldInfo subject = typeof(TestClassWithStaticMembers).GetField(nameof(TestClassWithStaticMembers.StaticField))!;

				async Task Act()
					=> await That(subject).IsNotStatic();

				await That(Act).ThrowsException()
					.WithMessage($"""
					              Expected that subject
					              is not static,
					              but it was static {Formatter.Format(subject)}
					              """);
			}
		}
	}
}
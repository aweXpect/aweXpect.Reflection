using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatField
{
	public sealed class IsNotPrivate
	{
		public sealed class Tests
		{
			[Theory]
			[InlineData("ProtectedField")]
			[InlineData("PublicField")]
			[InlineData("InternalField")]
			public async Task WhenFieldInfoIsNotPrivate_ShouldSucceed(string fieldName)
			{
				FieldInfo? subject = GetField(fieldName);

				async Task Act()
					=> await That(subject).IsNotPrivate();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenFieldInfoIsNull_ShouldFail()
			{
				FieldInfo? subject = null;

				async Task Act()
					=> await That(subject).IsNotPrivate();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is not private,
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenFieldInfoIsPrivate_ShouldFail()
			{
				FieldInfo? subject = GetField("PrivateField");

				async Task Act()
					=> await That(subject).IsNotPrivate();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             is not private,
					             but it was
					             """);
			}
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenFieldInfoIsPrivate_ShouldSucceed()
			{
				FieldInfo? subject = GetField("PrivateField");

				async Task Act()
					=> await That(subject).DoesNotComplyWith(it => it.IsNotPrivate());

				await That(Act).DoesNotThrow();
			}

			[Theory]
			[InlineData("ProtectedField")]
			[InlineData("PublicField")]
			[InlineData("InternalField")]
			public async Task WhenFieldInfoIsNotPrivate_ShouldFail(string fieldName)
			{
				FieldInfo? subject = GetField(fieldName);

				async Task Act()
					=> await That(subject).DoesNotComplyWith(it => it.IsNotPrivate());

				await That(Act).Throws<XunitException>()
					.WithMessage($"""
					             Expected that subject
					             is private,
					             but it was {GetExpectedAccessModifier(fieldName)}
					             """);
			}

			private static string GetExpectedAccessModifier(string fieldName) => fieldName switch
			{
				"ProtectedField" => "protected",
				"PublicField" => "public", 
				"PrivateField" => "private",
				"InternalField" => "internal",
				"ProtectedInternalField" => "protected internal",
				"PrivateProtectedField" => "private protected",
				_ => throw new ArgumentException($"Unknown field name: {fieldName}")
			};
		}
	}
}

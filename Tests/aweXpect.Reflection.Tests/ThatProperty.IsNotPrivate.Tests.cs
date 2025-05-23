﻿using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatProperty
{
	public sealed class IsNotPrivate
	{
		public sealed class Tests
		{
			[Theory]
			[InlineData("ProtectedProperty")]
			[InlineData("PublicProperty")]
			[InlineData("InternalProperty")]
			public async Task WhenPropertyInfoIsNotPrivate_ShouldSucceed(string propertyName)
			{
				PropertyInfo? subject = GetProperty(propertyName);

				async Task Act()
					=> await That(subject).IsNotPrivate();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenPropertyInfoIsNull_ShouldFail()
			{
				PropertyInfo? subject = null;

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
			public async Task WhenPropertyInfoIsPrivate_ShouldFail()
			{
				PropertyInfo? subject = GetProperty("PrivateProperty");

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
	}
}

﻿using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatEvent
{
	public sealed class IsNotPublic
	{
		public sealed class Tests
		{
			[Theory]
			[InlineData("ProtectedEvent")]
			[InlineData("InternalEvent")]
			[InlineData("PrivateEvent")]
			public async Task WhenEventInfoIsNotPublic_ShouldSucceed(string eventName)
			{
				EventInfo? subject = GetEvent(eventName);

				async Task Act()
					=> await That(subject).IsNotPublic();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenEventInfoIsNull_ShouldFail()
			{
				EventInfo? subject = null;

				async Task Act()
					=> await That(subject).IsNotPublic();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is not public,
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenEventInfoIsPublic_ShouldFail()
			{
				EventInfo? subject = GetEvent("PublicEvent");

				async Task Act()
					=> await That(subject).IsNotPublic();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             is not public,
					             but it was
					             """);
			}
		}
	}
}

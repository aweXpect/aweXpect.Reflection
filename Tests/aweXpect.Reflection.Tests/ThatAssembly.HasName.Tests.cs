﻿using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssembly
{
	public sealed class HasName
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenExpectedValueIsOnlySubstring_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasName("Reflection");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has name equal to "Reflection",
					             but it was "aweXpect.Reflection.Tests" which differs at index 0:
					                ↓ (actual)
					               "aweXpect.Reflection.Tests"
					               "Reflection"
					                ↑ (expected)
					             """);
			}

			[Fact]
			public async Task WhenTypeHasExpectedPrefix_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasName("aweXpect.Reflection").AsPrefix();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeHasMatchingName_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasName("aweXpect.Reflection.Tests");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeIsNull_ShouldFail()
			{
				Assembly? subject = null;

				async Task Act()
					=> await That(subject).HasName("foo");

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             has name equal to "foo",
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenTypeMatchesIgnoringCase_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasName("AWExPECT.rEFLECTION.tESTS").IgnoringCase();

				await That(Act).DoesNotThrow();
			}
		}
	}
}

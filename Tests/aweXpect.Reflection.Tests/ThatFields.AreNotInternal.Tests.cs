﻿using aweXpect.Reflection.Collections;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatFields
{
	public sealed class AreNotInternal
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenFieldInfoIsInternal_ShouldFail()
			{
				Filtered.Fields subject = GetFields("InternalField");

				async Task Act()
					=> await That(subject).AreNotInternal();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that *
					             all are not internal,
					             but it contained internal items [
					               *
					             ]
					             """).AsWildcard();
			}

			[Theory]
			[InlineData("ProtectedField")]
			[InlineData("PublicField")]
			[InlineData("PrivateField")]
			public async Task WhenFieldInfoIsNotInternal_ShouldFail(string fieldName)
			{
				Filtered.Fields subject = GetFields(fieldName);

				async Task Act()
					=> await That(subject).AreNotInternal();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenFieldInfoIsInternal_ShouldSucceed()
			{
				Filtered.Fields subject = GetFields("InternalField");

				async Task Act()
					=> await That(subject).DoesNotComplyWith(they => they.AreNotInternal());

				await That(Act).DoesNotThrow();
			}

			[Theory]
			[InlineData("ProtectedField")]
			[InlineData("PublicField")]
			[InlineData("PrivateField")]
			public async Task WhenFieldInfoIsNotInternal_ShouldFail(string fieldName)
			{
				Filtered.Fields subject = GetFields(fieldName);

				async Task Act()
					=> await That(subject).DoesNotComplyWith(they => they.AreNotInternal());

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that *
					             at least one is internal,
					             but none were
					             """).AsWildcard();
			}
		}
	}
}

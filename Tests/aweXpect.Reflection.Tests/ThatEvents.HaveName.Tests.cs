﻿using aweXpect.Reflection.Collections;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatEvents
{
	public sealed class HaveName
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenEventInfosContainEventInfoWithDifferentName_ShouldFail()
			{
				Filtered.Events subject = GetTypes<ThatEvent.ClassWithEvents>().Events();

				async Task Act()
					=> await That(subject).HaveName("PublicEvent");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that events in types matching t => t == typeof(T) in assembly containing type ThatEvent.ClassWithEvents
					             all have name equal to "PublicEvent",
					             but it contained not matching items [
					               *
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenEventInfosHaveName_ShouldSucceed()
			{
				Filtered.Events subject = GetTypes<ThatEvent.ClassWithSingleEvent>().Events();

				async Task Act()
					=> await That(subject).HaveName("MyEvent");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenEventInfosMatchIgnoringCase_ShouldSucceed()
			{
				Filtered.Events subject = GetTypes<ThatEvent.ClassWithSingleEvent>().Events();

				async Task Act()
					=> await That(subject).HaveName("mYevent").IgnoringCase();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenEventInfosMatchSuffix_ShouldSucceed()
			{
				Filtered.Events subject = GetTypes<ThatEvent.ClassWithEvents>().Events();

				async Task Act()
					=> await That(subject).HaveName("Event").AsSuffix();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenEventsDoNotHaveName_ShouldSucceed()
			{
				Filtered.Events subject = GetTypes<ThatEvent.ClassWithEvents>().Events();

				async Task Act()
					=> await That(subject).DoesNotComplyWith(they => they.HaveName("NonExistentEvent"));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenEventsHaveName_ShouldFail()
			{
				Filtered.Events subject = GetTypes<ThatEvent.ClassWithSingleEvent>().Events();

				async Task Act()
					=> await That(subject).DoesNotComplyWith(they => they.HaveName("MyEvent"));

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that events in types matching t => t == typeof(T) in assembly containing type ThatEvent.ClassWithSingleEvent
					             all have name not equal to "MyEvent",
					             but it only contained matching items *
					             """).AsWildcard();
			}
		}
	}
}

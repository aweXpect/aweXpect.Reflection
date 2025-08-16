using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatEvents
{
	public sealed class Have
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenAllEventsHaveAttribute_ShouldSucceed()
			{
				IEnumerable<EventInfo> subject = new[]
				{
					typeof(TestClass).GetEvent("TestEvent1")!,
					typeof(TestClass).GetEvent("TestEvent2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllEventsHaveMatchingAttribute_ShouldSucceed()
			{
				IEnumerable<EventInfo> subject = new[]
				{
					typeof(TestClass).GetEvent("TestEvent1")!,
					typeof(TestClass).GetEvent("TestEvent2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value.StartsWith("Event"));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenNotAllEventsHaveAttribute_ShouldFail()
			{
				IEnumerable<EventInfo> subject = new[]
				{
					typeof(TestClass).GetEvent("TestEvent1")!,
					typeof(TestClass).GetEvent("NoAttributeEvent")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have ThatEvents.Have.AttributeTests.TestAttribute,
					             but it contained not matching events [
					               System.Action NoAttributeEvent
					             ]
					             """);
			}

			[Fact]
			public async Task WhenNotAllEventsHaveMatchingAttribute_ShouldFail()
			{
				IEnumerable<EventInfo> subject = new[]
				{
					typeof(TestClass).GetEvent("TestEvent1")!,
					typeof(TestClass).GetEvent("TestEvent2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value == "WrongValue");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have ThatEvents.Have.AttributeTests.TestAttribute matching attr => attr.Value == "WrongValue",
					             but it contained not matching events [
					               System.Action TestEvent1,
					               System.Action TestEvent2
					             ]
					             """);
			}

			[AttributeUsage(AttributeTargets.Event)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			private class TestClass
			{
				[Test(Value = "Event1Value")]
				public event Action? TestEvent1;

				[Test(Value = "Event2Value")]
				public event Action? TestEvent2;

				public event Action? NoAttributeEvent;
			}
		}
	}
}
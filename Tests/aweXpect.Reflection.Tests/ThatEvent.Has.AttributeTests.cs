using System;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatEvent
{
	public sealed class Has
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenEventHasAttribute_ShouldSucceed()
			{
				EventInfo subject = typeof(TestClass).GetEvent("TestEvent")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[AttributeUsage(AttributeTargets.Event)]
			private class TestAttribute : Attribute
			{
			}

			private class TestClass
			{
				[Test]
				public event Action? TestEvent;

				public event Action? NoAttributeEvent;
			}
		}
	}
}
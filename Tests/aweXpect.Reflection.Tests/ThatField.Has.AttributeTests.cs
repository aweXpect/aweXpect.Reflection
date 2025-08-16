using System;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatField
{
	public sealed class Has
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenFieldHasAttribute_ShouldSucceed()
			{
				FieldInfo subject = typeof(TestClass).GetField("TestField")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[AttributeUsage(AttributeTargets.Field)]
			private class TestAttribute : Attribute
			{
			}

			private class TestClass
			{
				[Test]
				public string TestField = "";

				public string NoAttributeField = "";
			}
		}
	}
}
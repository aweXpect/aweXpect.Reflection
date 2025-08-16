using System;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatProperty
{
	public sealed class Has
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenPropertyHasAttribute_ShouldSucceed()
			{
				PropertyInfo subject = typeof(TestClass).GetProperty("TestProperty")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenPropertyHasMatchingAttribute_ShouldSucceed()
			{
				PropertyInfo subject = typeof(TestClass).GetProperty("TestProperty")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>(attr => attr.Value == "PropertyValue");

				await That(Act).DoesNotThrow();
			}

			[AttributeUsage(AttributeTargets.Property)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			private class TestClass
			{
				[Test(Value = "PropertyValue")]
				public string TestProperty { get; set; } = "";

				public string NoAttributeProperty { get; set; } = "";
			}
		}
	}
}
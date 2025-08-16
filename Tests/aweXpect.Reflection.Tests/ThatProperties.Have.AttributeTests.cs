using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatProperties
{
	public sealed class Have
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenAllPropertiesHaveAttribute_ShouldSucceed()
			{
				IEnumerable<PropertyInfo> subject = new[]
				{
					typeof(TestClass).GetProperty("TestProperty1")!,
					typeof(TestClass).GetProperty("TestProperty2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllPropertiesHaveMatchingAttribute_ShouldSucceed()
			{
				IEnumerable<PropertyInfo> subject = new[]
				{
					typeof(TestClass).GetProperty("TestProperty1")!,
					typeof(TestClass).GetProperty("TestProperty2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value.StartsWith("Property"));

				await That(Act).DoesNotThrow();
			}

			[AttributeUsage(AttributeTargets.Property)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			private class TestClass
			{
				[Test(Value = "Property1Value")]
				public string TestProperty1 { get; set; } = "";

				[Test(Value = "Property2Value")]
				public string TestProperty2 { get; set; } = "";

				public string NoAttributeProperty { get; set; } = "";
			}
		}
	}
}
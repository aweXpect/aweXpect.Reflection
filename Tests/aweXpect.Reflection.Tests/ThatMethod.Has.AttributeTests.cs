using System;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class Has
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenMethodHasAttribute_ShouldSucceed()
			{
				MethodInfo subject = typeof(TestClass).GetMethod("TestMethod")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenMethodHasMatchingAttribute_ShouldSucceed()
			{
				MethodInfo subject = typeof(TestClass).GetMethod("TestMethod")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>(attr => attr.Value == "MethodValue");

				await That(Act).DoesNotThrow();
			}

			[AttributeUsage(AttributeTargets.Method)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			private class TestClass
			{
				[Test(Value = "MethodValue")]
				public void TestMethod() { }

				public void NoAttributeMethod() { }
			}
		}
	}
}
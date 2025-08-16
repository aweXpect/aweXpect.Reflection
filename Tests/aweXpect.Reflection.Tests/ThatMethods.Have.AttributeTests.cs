using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethods
{
	public sealed class Have
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenAllMethodsHaveAttribute_ShouldSucceed()
			{
				IEnumerable<MethodInfo> subject = new[]
				{
					typeof(TestClass).GetMethod("TestMethod1")!,
					typeof(TestClass).GetMethod("TestMethod2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllMethodsHaveMatchingAttribute_ShouldSucceed()
			{
				IEnumerable<MethodInfo> subject = new[]
				{
					typeof(TestClass).GetMethod("TestMethod1")!,
					typeof(TestClass).GetMethod("TestMethod2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value.StartsWith("Method"));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenNotAllMethodsHaveAttribute_ShouldFail()
			{
				IEnumerable<MethodInfo> subject = new[]
				{
					typeof(TestClass).GetMethod("TestMethod1")!,
					typeof(TestClass).GetMethod("NoAttributeMethod")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have ThatMethods.Have.AttributeTests.TestAttribute,
					             but it contained not matching methods [
					               Void NoAttributeMethod()
					             ]
					             """);
			}

			[Fact]
			public async Task WhenNotAllMethodsHaveMatchingAttribute_ShouldFail()
			{
				IEnumerable<MethodInfo> subject = new[]
				{
					typeof(TestClass).GetMethod("TestMethod1")!,
					typeof(TestClass).GetMethod("TestMethod2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value == "WrongValue");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have ThatMethods.Have.AttributeTests.TestAttribute matching attr => attr.Value == "WrongValue",
					             but it contained not matching methods [
					               Void TestMethod1(),
					               Void TestMethod2()
					             ]
					             """);
			}

			[AttributeUsage(AttributeTargets.Method)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			private class TestClass
			{
				[Test(Value = "Method1Value")]
				public void TestMethod1() { }

				[Test(Value = "Method2Value")]
				public void TestMethod2() { }

				public void NoAttributeMethod() { }
			}
		}
	}
}
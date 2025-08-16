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

			[Fact]
			public async Task WhenFieldHasMatchingAttribute_ShouldSucceed()
			{
				FieldInfo subject = typeof(TestClass).GetField("TestFieldWithValue")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>(attr => attr.Value == 42);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenFieldDoesNotHaveAttribute_ShouldFail()
			{
				FieldInfo subject = typeof(TestClass).GetField("NoAttributeField")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has ThatField.Has.AttributeTests.TestAttribute,
					             but it did not in System.String NoAttributeField
					             """);
			}

			[Fact]
			public async Task WhenFieldHasNotMatchingAttribute_ShouldFail()
			{
				FieldInfo subject = typeof(TestClass).GetField("TestFieldWithValue")!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>(attr => attr.Value == 99);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has ThatField.Has.AttributeTests.TestAttribute matching attr => attr.Value == 99,
					             but it did not in System.String TestFieldWithValue
					             """);
			}

			[Fact]
			public async Task WhenFieldIsNull_ShouldFail()
			{
				FieldInfo? subject = null;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             has ThatField.Has.AttributeTests.TestAttribute,
					             but it was <null>
					             """);
			}

			[AttributeUsage(AttributeTargets.Field)]
			private class TestAttribute : Attribute
			{
				public int Value { get; set; }
			}

			private class TestClass
			{
				[Test]
				public string TestField = "";

				[Test(Value = 42)]
				public string TestFieldWithValue = "";

				public string NoAttributeField = "";
			}
		}
	}
}
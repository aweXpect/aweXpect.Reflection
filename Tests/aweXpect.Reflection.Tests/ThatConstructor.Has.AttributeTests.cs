using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatConstructor
{
	public sealed class Has
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenConstructorDoesNotHaveAttribute_ShouldFail()
			{
				ConstructorInfo subject = typeof(TestClass).GetConstructor(Type.EmptyTypes)!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has ThatConstructor.Has.AttributeTests.TestAttribute,
					             but it did not in Void .ctor()
					             """);
			}

			[Fact]
			public async Task WhenConstructorHasAttribute_ShouldSucceed()
			{
				ConstructorInfo subject = typeof(TestClass).GetConstructor([typeof(string)])!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenConstructorHasMatchingAttribute_ShouldSucceed()
			{
				ConstructorInfo subject = typeof(TestClass).GetConstructor([typeof(string)])!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>(attr => attr.Value == "ConstructorValue");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenConstructorHasNotMatchingAttribute_ShouldFail()
			{
				ConstructorInfo subject = typeof(TestClass).GetConstructor([typeof(string)])!;

				async Task Act()
					=> await That(subject).Has<TestAttribute>(attr => attr.Value == "WrongValue");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has ThatConstructor.Has.AttributeTests.TestAttribute matching attr => attr.Value == "WrongValue",
					             but it did not in Void .ctor(System.String)
					             """);
			}

			[Fact]
			public async Task WhenConstructorIsNull_ShouldFail()
			{
				ConstructorInfo? subject = null;

				async Task Act()
					=> await That(subject).Has<TestAttribute>();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             has ThatConstructor.Has.AttributeTests.TestAttribute,
					             but it was <null>
					             """);
			}

			[AttributeUsage(AttributeTargets.Constructor)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public TestClass() { }

				[Test(Value = "ConstructorValue")]
				public TestClass(string value) { }
			}
			// ReSharper restore UnusedMember.Local
		}
	}

	public sealed class NegatedTests
	{
		[Fact]
		public async Task WhenConstructorDoesNotHaveAttribute_ShouldSucceed()
		{
			ConstructorInfo subject = typeof(TestClass).GetConstructor(Type.EmptyTypes)!;

			async Task Act()
				=> await That(subject).DoesNotComplyWith(it => it.Has<TestAttribute>());

			await That(Act).DoesNotThrow();
		}

		[Fact]
		public async Task WhenConstructorDoesNotHaveMatchingAttribute_ShouldSucceed()
		{
			ConstructorInfo subject = typeof(TestClass).GetConstructor([typeof(string)])!;

			async Task Act()
				=> await That(subject).DoesNotComplyWith(it => it.Has<TestAttribute>(attr => attr.Value == "NonExistent"));

			await That(Act).DoesNotThrow();
		}

		[Fact]
		public async Task WhenConstructorHasAttribute_ShouldFail()
		{
			ConstructorInfo subject = typeof(TestClass).GetConstructor([typeof(string)])!;

			async Task Act()
				=> await That(subject).DoesNotComplyWith(it => it.Has<TestAttribute>());

			await That(Act).Throws<XunitException>()
				.WithMessage("""
				             Expected that subject
				             has no ThatConstructor.NegatedTests.TestAttribute,
				             but it did in Void .ctor(System.String)
				             """);
		}

		[AttributeUsage(AttributeTargets.Constructor)]
		private class TestAttribute : Attribute
		{
			public string Value { get; set; } = "";
		}

		// ReSharper disable UnusedMember.Local
		private class TestClass
		{
			public TestClass() { }

			[Test(Value = "ConstructorValue")]
			public TestClass(string value) { }
		}
		// ReSharper restore UnusedMember.Local
	}
}
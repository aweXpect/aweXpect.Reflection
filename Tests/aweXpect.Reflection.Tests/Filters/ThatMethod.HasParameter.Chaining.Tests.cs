using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ThatMethod
{
	public sealed class HasParameterChaining
	{
		public sealed class Tests
		{
			[Fact]
			public async Task AtIndex_WhenParameterExistsAtSpecificIndex_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>().AtIndex(0);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task AtIndex_WhenParameterDoesNotExistAtSpecificIndex_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<string>().AtIndex(0);

				await That(Act).Throws<XunitException>()
					.WithMessage("*has parameter of type System.String*");
			}

			[Fact]
			public async Task AtIndexFromEnd_WhenParameterExistsAtSpecificIndexFromEnd_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<string>().AtIndex(1).FromEnd();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WithoutDefaultValue_WhenParameterHasNoDefault_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>().WithoutDefaultValue();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WithDefaultValue_WhenParameterHasDefault_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithDefaults))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<string>().WithDefaultValue();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task IgnoringCase_WhenParameterNameDiffersInCase_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>("VALUE").IgnoringCase();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task AsPrefix_WhenParameterNameStartsWithPrefix_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>("val").AsPrefix();

				await That(Act).DoesNotThrow();
			}

#pragma warning disable CA1822
			// ReSharper disable UnusedParameter.Local
			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public void MethodWithIntAndString(int value, string name) { }
				public void MethodWithDefaults(int value, string name = "default") { }
			}
			// ReSharper restore UnusedParameter.Local
			// ReSharper restore UnusedMember.Local
#pragma warning restore CA1822
		}
	}
}
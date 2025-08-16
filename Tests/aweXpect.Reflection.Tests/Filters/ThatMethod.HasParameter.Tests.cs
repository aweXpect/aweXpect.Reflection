using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ThatMethod
{
	public sealed class HasParameter
	{
		public sealed class Tests
		{
			[Fact]
			public async Task HasParameterByType_WhenParameterExists_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByType_WhenParameterDoesNotExist_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithoutParameters))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>();

				await That(Act).Throws<XunitException>()
					.WithMessage("*has parameter of type System.Int32*");
			}

			[Fact]
			public async Task HasParameterByName_WhenParameterExists_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByName_WhenParameterDoesNotExist_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithoutParameters))!;

				async Task Act()
					=> await That(methodInfo).HasParameter("value");

				await That(Act).ThrowsException()
					;
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenParameterExists_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenParameterDoesNotExist_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithoutParameters))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>("value");

				await That(Act).ThrowsException()
					;
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenWrongType_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<string>("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("*has parameter of type System.String with name \"value\"*");
			}

#pragma warning disable CA1822
			// ReSharper disable UnusedParameter.Local
			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public void MethodWithoutParameters() { }
				public void MethodWithString(string name) { }
				public void MethodWithIntAndString(int value, string name) { }
			}
			// ReSharper restore UnusedParameter.Local
			// ReSharper restore UnusedMember.Local
#pragma warning restore CA1822
		}
	}
}
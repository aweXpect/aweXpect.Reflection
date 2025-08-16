using System.Reflection;

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
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method2))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByType_WhenParameterDoesNotExist_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method1), Type.EmptyTypes)!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>();

				await That(Act).ThrowsException()
					.WithMessage("*has parameter of type int*but it did not");
			}

			[Fact]
			public async Task HasParameterByName_WhenParameterExists_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method2))!;

				async Task Act()
					=> await That(methodInfo).HasParameter("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByName_WhenParameterDoesNotExist_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method1), Type.EmptyTypes)!;

				async Task Act()
					=> await That(methodInfo).HasParameter("value");

				await That(Act).ThrowsException()
					.WithMessage("*has parameter with name \"value\"*but it did not");
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenParameterExists_ShouldSucceed()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method2))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenParameterDoesNotExist_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method1), Type.EmptyTypes)!;

				async Task Act()
					=> await That(methodInfo).HasParameter<int>("value");

				await That(Act).ThrowsException()
					.WithMessage("*has parameter of type int with name \"value\"*but it did not");
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenWrongType_ShouldFail()
			{
				MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method2))!;

				async Task Act()
					=> await That(methodInfo).HasParameter<string>("value");

				await That(Act).ThrowsException()
					.WithMessage("*has parameter of type string with name \"value\"*but it did not");
			}

#pragma warning disable CA1822
			// ReSharper disable UnusedParameter.Local
			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public void Method1() { }
				public void Method1(string name) { }
				public void Method2(int value, string name) { }
			}
			// ReSharper restore UnusedParameter.Local
			// ReSharper restore UnusedMember.Local
#pragma warning restore CA1822
		}
	}
}
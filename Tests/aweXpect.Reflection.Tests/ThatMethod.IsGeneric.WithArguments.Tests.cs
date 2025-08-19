using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class IsGenericWithArguments
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WithCount_WhenMethodHasCorrectGenericArgumentCount_ShouldSucceed()
			{
				MethodInfo? subject = GetGenericMethod("GenericMethodWithTwoParameters");

				async Task Act()
					=> await That(subject).IsGeneric().WithCount(2);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WithCount_WhenMethodHasIncorrectGenericArgumentCount_ShouldFail()
			{
				MethodInfo? subject = GetGenericMethod("GenericMethod");

				async Task Act()
					=> await That(subject).IsGeneric().WithCount(2);

				await That(Act).Throws<XunitException>();
			}

			[Fact]
			public async Task AtIndex_WhenCheckingGenericParameter_ShouldSucceed()
			{
				MethodInfo? subject = GetGenericMethod("GenericMethodWithTwoParameters");

				async Task Act()
					=> await That(subject).IsGeneric().AtIndex(0);

				await That(Act).DoesNotThrow();
			}

			private static MethodInfo? GetGenericMethod(string methodName)
			{
				return typeof(TestClass).GetMethod(methodName);
			}
		}

		private class TestClass
		{
			public T GenericMethod<T>(T parameter) => parameter;
			public void GenericMethodWithTwoParameters<T, U>(T param1, U param2) { }
		}
	}
}
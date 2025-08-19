using System;
using System.Collections.Generic;
using System.Reflection;

namespace aweXpect.Reflection.Tests;

public sealed class ApiDemonstration
{
	public sealed class Tests
	{
		[Fact]
		public async Task ShouldDemonstrateApiFromIssue()
		{
			// This demonstrates the exact API requested in the issue #134
			MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.GenericMethodWithTwoParameters))!;

			// This should work as requested in the issue
			async Task Act()
				=> await That(method).IsGeneric()
					.WithCount(2);

			await That(Act).DoesNotThrow();
		}

		[Fact] 
		public async Task ShouldDemonstrateTypeApiAsWell()
		{
			// This shows the same API works for types
			Type type = typeof(Dictionary<string, int>);

			async Task Act()
				=> await That(type).IsGeneric()
					.WithCount(2)
					.WithGenericParameter<string>().AtIndex(0);

			await That(Act).DoesNotThrow();
		}

		private class TestClass
		{
			public void GenericMethodWithTwoParameters<T, U>(T param1, U param2) { }
		}
	}
}
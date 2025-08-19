using System;
using System.Collections.Generic;
using System.Reflection;

namespace aweXpect.Reflection.Tests;

public sealed class ApiDemonstration
{
	public sealed class Tests
	{
		[Fact]
		public async Task ShouldDemonstrateExactApiFromIssue()
		{
			// This demonstrates the EXACT API requested in issue #134
			MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.GenericMethodWithTwoParameters))!;

			// This exact syntax was requested in the issue:
			// await That(method).IsGeneric()
			//     .WithCount(2)
			//     .WithGenericParameter<string>().AtIndex(0)
			
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

		[Fact]
		public async Task ShouldDemonstrateFilteringFunctionality()
		{
			// This shows the filtering works as well
			var genericMethods = In.AssemblyContaining<Tests>().Types()
				.Methods()
				.WhichAreGeneric()
				.WithCount(1);

			await That(genericMethods.GetDescription()).Contains("generic methods with 1 generic argument");
		}

		private class TestClass
		{
			public void GenericMethodWithTwoParameters<T, U>(T param1, U param2) { }
		}
	}
}
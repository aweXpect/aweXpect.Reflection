using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethods
{
	public sealed class HaveParameter
	{
		public sealed class Tests
		{
			[Fact]
			public async Task HaveParameterByType_WhenAllHaveParameter_ShouldSucceed()
			{
				IEnumerable<MethodInfo> methods = new[]
				{
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!,
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithInt))!
				};

				async Task Act()
					=> await That(methods).HaveParameter<int>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HaveParameterByType_WhenNotAllHaveParameter_ShouldFail()
			{
				IEnumerable<MethodInfo> methods = new[]
				{
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!,
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithString))! // No int parameter
				};

				async Task Act()
					=> await That(methods).HaveParameter<int>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods
					             all have parameter of type int,
					             but at least one did not
					             """);
			}

			[Fact]
			public async Task HaveParameterByName_WhenAllHaveParameter_ShouldSucceed()
			{
				IEnumerable<MethodInfo> methods = new[]
				{
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!,
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithInt))!
				};

				async Task Act()
					=> await That(methods).HaveParameter("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HaveParameterByName_WhenNotAllHaveParameter_ShouldFail()
			{
				IEnumerable<MethodInfo> methods = new[]
				{
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!,
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithString))! // Has "name" parameter, not "value"
				};

				async Task Act()
					=> await That(methods).HaveParameter("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods
					             all have parameter with name "value",
					             but at least one did not
					             """);
			}

			[Fact]
			public async Task HaveParameterByTypeAndName_WhenAllHaveParameter_ShouldSucceed()
			{
				IEnumerable<MethodInfo> methods = new[]
				{
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!,
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithInt))!
				};

				async Task Act()
					=> await That(methods).HaveParameter<int>("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HaveParameterByTypeAndName_WhenNotAllHaveParameter_ShouldFail()
			{
				IEnumerable<MethodInfo> methods = new[]
				{
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithIntAndString))!,
					typeof(TestClass).GetMethod(nameof(TestClass.MethodWithString))! // Has string "name", not int "value"
				};

				async Task Act()
					=> await That(methods).HaveParameter<int>("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods
					             all have parameter of type int with name "value",
					             but at least one did not
					             """);
			}

#pragma warning disable CA1822
			// ReSharper disable UnusedParameter.Local
			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public void MethodWithInt(int value) { }
				public void MethodWithString(string name) { }
				public void MethodWithIntAndString(int value, string name) { }
			}
			// ReSharper restore UnusedParameter.Local
			// ReSharper restore UnusedMember.Local
#pragma warning restore CA1822
		}
	}
}

using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatConstructors
{
	public sealed class HaveParameter
	{
		public sealed class Tests
		{
			[Fact]
			public async Task HaveParameterByType_WhenAllHaveParameter_ShouldSucceed()
			{
				IEnumerable<ConstructorInfo> constructors = new[]
				{
					typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClass).GetConstructor([typeof(int)])!
				};

				async Task Act()
					=> await That(constructors).HaveParameter<int>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HaveParameterByType_WhenNotAllHaveParameter_ShouldFail()
			{
				IEnumerable<ConstructorInfo> constructors = new[]
				{
					typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClass).GetConstructor([typeof(string)])! // No int parameter
				};

				async Task Act()
					=> await That(constructors).HaveParameter<int>();

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructors\nhave parameter of type int,\nbut at least one did not");
			}

			[Fact]
			public async Task HaveParameterByName_WhenAllHaveParameter_ShouldSucceed()
			{
				IEnumerable<ConstructorInfo> constructors = new[]
				{
					typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClass).GetConstructor([typeof(int)])!
				};

				async Task Act()
					=> await That(constructors).HaveParameter("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HaveParameterByName_WhenNotAllHaveParameter_ShouldFail()
			{
				IEnumerable<ConstructorInfo> constructors = new[]
				{
					typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClass).GetConstructor([typeof(string)])! // Has "name" parameter, not "value"
				};

				async Task Act()
					=> await That(constructors).HaveParameter("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructors\nhave parameter with name \"value\",\nbut at least one did not");
			}

			[Fact]
			public async Task HaveParameterByTypeAndName_WhenAllHaveParameter_ShouldSucceed()
			{
				IEnumerable<ConstructorInfo> constructors = new[]
				{
					typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClass).GetConstructor([typeof(int)])!
				};

				async Task Act()
					=> await That(constructors).HaveParameter<int>("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HaveParameterByTypeAndName_WhenNotAllHaveParameter_ShouldFail()
			{
				IEnumerable<ConstructorInfo> constructors = new[]
				{
					typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClass).GetConstructor([typeof(string)])! // Has string "name", not int "value"
				};

				async Task Act()
					=> await That(constructors).HaveParameter<int>("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructors\nhave parameter of type int with name \"value\",\nbut at least one did not");
			}

			// ReSharper disable UnusedParameter.Local
			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public TestClass(int value) { }
				public TestClass(string name) { }
				public TestClass(int value, string name) { }
			}
			// ReSharper restore UnusedParameter.Local
			// ReSharper restore UnusedMember.Local
		}
	}
}
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatConstructor
{
	public sealed class HasParameter
	{
		public sealed class Tests
		{
			[Fact]
			public async Task HasParameterByType_WhenParameterExists_ShouldSucceed()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter<int>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByType_WhenParameterDoesNotExist_ShouldFail()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter<int>();

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructorInfo\nhas parameter of type int,\nbut it did not");
			}

			[Fact]
			public async Task HasParameterByName_WhenParameterExists_ShouldSucceed()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByName_WhenParameterDoesNotExist_ShouldFail()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructorInfo\nhas parameter with name \"value\",\nbut it did not");
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenParameterExists_ShouldSucceed()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter<int>("value");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenParameterDoesNotExist_ShouldFail()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter<int>("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructorInfo\nhas parameter of type int with name \"value\",\nbut it did not");
			}

			[Fact]
			public async Task HasParameterByTypeAndName_WhenWrongType_ShouldFail()
			{
				ConstructorInfo constructorInfo = typeof(TestClass).GetConstructor([typeof(int), typeof(string)])!;

				async Task Act()
					=> await That(constructorInfo).HasParameter<string>("value");

				await That(Act).Throws<XunitException>()
					.WithMessage("Expected that constructorInfo\nhas parameter of type string with name \"value\",\nbut it did not");
			}

			// ReSharper disable UnusedParameter.Local
			// ReSharper disable UnusedMember.Local
			private class TestClass
			{
				public TestClass() { }
				public TestClass(string name) { }
				public TestClass(int value, string name) { }
			}
			// ReSharper restore UnusedParameter.Local
			// ReSharper restore UnusedMember.Local
		}
	}
}
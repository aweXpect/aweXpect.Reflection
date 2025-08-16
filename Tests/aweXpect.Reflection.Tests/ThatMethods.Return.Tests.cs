using System;
using System.Reflection;
using System.Threading.Tasks;
using aweXpect.Reflection.Collections;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethods
{
	public sealed class Return
	{
		public sealed class GenericTests
		{
			[Fact]
			public async Task ShouldSucceedWhenAllMethodsReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString));

				async Task Act()
					=> await That(methods).Return<string>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task ShouldFailWhenSomeMethodsDoNotReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return<string>();

				await That(Act).Throws<XunitException>()
					.WithMessage("*all return string*").AsWildcard();
			}

			[Fact]
			public async Task ShouldSucceedWithInheritance()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

				async Task Act()
					=> await That(methods).Return<DummyBase>();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class TypeTests
		{
			[Fact]
			public async Task ShouldSucceedWhenAllMethodsReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString));

				async Task Act()
					=> await That(methods).Return(typeof(string));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task ShouldFailWhenSomeMethodsDoNotReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return(typeof(string));

				await That(Act).Throws<XunitException>()
					.WithMessage("*all return string*").AsWildcard();
			}
		}

#pragma warning disable CA1822 // Mark members as static
		private class TestClass
		{
			public string GetString() => "test";
			public int GetInt() => 42;
			public bool GetBool() => true;
			public DummyBase GetDummyBase() => new();
			public Dummy GetDummy() => new();
			public async Task AsyncMethod() => await Task.CompletedTask;
			public async ValueTask AsyncValueTaskMethod() => await ValueTask.CompletedTask;
		}
#pragma warning restore CA1822

		private class DummyBase
		{
		}

		private class Dummy : DummyBase
		{
		}

		public sealed class OrReturnGenericTests
		{
			[Fact]
			public async Task ShouldSucceedWhenMethodsReturnAnyOfMultipleTypes()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get") && (m.ReturnType == typeof(string) || m.ReturnType == typeof(int) || m.ReturnType == typeof(bool)));

				async Task Act()
					=> await That(methods).Return<string>().OrReturn<int>().OrReturn<bool>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task ShouldFailWhenSomeMethodsDoNotReturnAnyOfTheSpecifiedTypes()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return<string>().OrReturn<int>();

				await That(Act).Throws<XunitException>()
					.WithMessage("*all return string or return Int32*").AsWildcard();
			}

			[Fact]
			public async Task ShouldSucceedWithTaskAndValueTask()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.AsyncMethod) || m.Name == nameof(TestClass.AsyncValueTaskMethod));

				async Task Act()
					=> await That(methods).Return<Task>().OrReturn<ValueTask>();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class OrReturnTests
		{
			[Fact]
			public async Task ShouldSucceedWhenMethodsReturnAnyOfMultipleTypes()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get") && (m.ReturnType == typeof(string) || m.ReturnType == typeof(int) || m.ReturnType == typeof(bool)));

				async Task Act()
					=> await That(methods).Return(typeof(string)).OrReturn(typeof(int)).OrReturn(typeof(bool));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task ShouldFailWhenSomeMethodsDoNotReturnAnyOfTheSpecifiedTypes()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return(typeof(string)).OrReturn(typeof(int));

				await That(Act).Throws<XunitException>()
					.WithMessage("*all return string or return Int32*").AsWildcard();
			}
		}
	}
}
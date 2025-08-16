using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class MethodFilters
{
	public sealed class WithParameter
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WithParameterOfType_ShouldFilterForMethodsWithParameterOfSpecificType()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter<string>();

				await That(methods).IsEqualTo([
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method1), [typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method2), [typeof(int), typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method4), [typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterOfTypeAndName_ShouldFilterForMethodsWithParameterOfSpecificTypeAndName()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter<string>("name");

				await That(methods).IsEqualTo([
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method1), [typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method2), [typeof(int), typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method4), [typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterByName_ShouldFilterForMethodsWithParameterOfSpecificName()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter("name");

				await That(methods).IsEqualTo([
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method1), [typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method2), [typeof(int), typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method4), [typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterAtIndex_ShouldFilterForMethodsWithParameterAtSpecificIndex()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter<int>().AtIndex(0);

				await That(methods).IsEqualTo([
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method2), [typeof(int), typeof(string)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method3), [typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterWithDefaultValue_ShouldFilterForMethodsWithParameterWithDefaultValue()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter<int>().WithDefaultValue();

				await That(methods).IsEqualTo([
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method3), [typeof(int)])!,
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method4), [typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterWithSpecificDefaultValue_ShouldFilterForMethodsWithParameterWithSpecificDefaultValue()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter<int>().WithDefaultValue(42);

				await That(methods).IsEqualTo([
					typeof(TestClassWithParameterMethods).GetMethod(nameof(TestClassWithParameterMethods.Method3), [typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task GetDescription_ShouldReturnCorrectDescription()
			{
				Filtered.Methods methods = In.Type<TestClassWithParameterMethods>()
					.Methods().WithParameter<string>();

				await That(methods.GetDescription())
					.IsEqualTo("methods with parameter of type string in").AsPrefix();
			}
		}

		// Test classes for parameter filtering scenarios
		public class TestClassWithParameterMethods
		{
			public void Method1() { }
			public void Method1(string name) { }
			public void Method2(int id, string name) { }
			public void Method3(int id = 42) { }
			public void Method4(string name, int value = 100) { }
		}
	}
}
using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ConstructorFilters
{
	public sealed class WithParameter
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WithParameterOfType_ShouldFilterForConstructorsWithParameterOfSpecificType()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter<string>();

				await That(constructors).IsEqualTo([
					typeof(TestClassWithParameters).GetConstructor([typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterOfTypeAndName_ShouldFilterForConstructorsWithParameterOfSpecificTypeAndName()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter<string>("name");

				await That(constructors).IsEqualTo([
					typeof(TestClassWithParameters).GetConstructor([typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterByName_ShouldFilterForConstructorsWithParameterOfSpecificName()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter("name");

				await That(constructors).IsEqualTo([
					typeof(TestClassWithParameters).GetConstructor([typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterAtIndex_ShouldFilterForConstructorsWithParameterAtSpecificIndex()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter<int>().AtIndex(0);

				await That(constructors).IsEqualTo([
					typeof(TestClassWithParameters).GetConstructor([typeof(int), typeof(string)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterWithDefaultValue_ShouldFilterForConstructorsWithParameterWithDefaultValue()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter<int>().WithDefaultValue();

				await That(constructors).IsEqualTo([
					typeof(TestClassWithParameters).GetConstructor([typeof(int)])!,
					typeof(TestClassWithParameters).GetConstructor([typeof(string), typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task WithParameterWithSpecificDefaultValue_ShouldFilterForConstructorsWithParameterWithSpecificDefaultValue()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter<int>().WithDefaultValue(42);

				await That(constructors).IsEqualTo([
					typeof(TestClassWithParameters).GetConstructor([typeof(int)])!
				]).InAnyOrder();
			}

			[Fact]
			public async Task GetDescription_ShouldReturnCorrectDescription()
			{
				Filtered.Constructors constructors = In.Type<TestClassWithParameters>()
					.Constructors().WithParameter<string>();

				await That(constructors.GetDescription())
					.IsEqualTo("constructors with parameter of type string in").AsPrefix();
			}
		}

		// Test classes for parameter filtering scenarios
		public class TestClassWithParameters
		{
			public TestClassWithParameters() { }
			public TestClassWithParameters(string name) { }
			public TestClassWithParameters(int id, string name) { }
			public TestClassWithParameters(int id = 42) { }
			public TestClassWithParameters(string name, int value = 100) { }
		}
	}
}
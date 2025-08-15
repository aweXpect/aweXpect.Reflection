using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class MethodFilters
{
	public sealed class WithParameter
	{
		public sealed class ParameterTests
		{
			[Fact]
			public async Task ShouldFilterForMethodsWithParameterMatchingPredicate()
			{
				Filtered.Methods methods = In.Type<Dummy>()
					.Methods().WithParameter(p => p.Name == "value");

				await That(methods).IsEqualTo([
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod1))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod2))!,
				]).InAnyOrder();
				await That(methods.GetDescription())
					.IsEqualTo("methods with parameter matching p => p.Name == \"value\"")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForMethodsWithParameterOfSpecificType()
			{
				Filtered.Methods methods = In.Type<Dummy>()
					.Methods().WithParameter(p => p.ParameterType == typeof(string));

				await That(methods).IsEqualTo([
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod2))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod3))!,
				]).InAnyOrder();
				await That(methods.GetDescription())
					.IsEqualTo("methods with parameter matching p => p.ParameterType == typeof(string)")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForMethodsWithParameterAtSpecificPosition()
			{
				Filtered.Methods methods = In.Type<Dummy>()
					.Methods().WithParameter(p => p.Position == 0);

				await That(methods).IsEqualTo([
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod1))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod2))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod3))!,
				]).InAnyOrder();
				await That(methods.GetDescription())
					.IsEqualTo("methods with parameter matching p => p.Position == 0")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForMethodsWithNoParameters()
			{
				Filtered.Methods methods = In.Type<Dummy>()
					.Methods().WithParameter(p => false);

				await That(methods).IsEmpty();
				await That(methods.GetDescription())
					.IsEqualTo("methods with parameter matching p => false")
					.AsPrefix();
			}
		}

		public sealed class OrWithParameterTests
		{
			[Fact]
			public async Task ShouldFilterForMethodsWithParameterMatchingEitherPredicate()
			{
				Filtered.Methods methods = In.Type<Dummy>()
					.Methods().WithParameter(p => p.ParameterType == typeof(int))
					.OrWithParameter(p => p.ParameterType == typeof(string));

				await That(methods).IsEqualTo([
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod1))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod2))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod3))!,
				]).InAnyOrder();
				await That(methods.GetDescription())
					.IsEqualTo(
						"methods with parameter matching p => p.ParameterType == typeof(int) " +
						"or with parameter matching p => p.ParameterType == typeof(string)")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForMethodsWithParameterAtDifferentPositions()
			{
				Filtered.Methods methods = In.Type<Dummy>()
					.Methods().WithParameter(p => p.Position == 0 && p.ParameterType == typeof(int))
					.OrWithParameter(p => p.Position == 1 && p.ParameterType == typeof(string));

				await That(methods).IsEqualTo([
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod1))!,
					typeof(Dummy).GetMethod(nameof(Dummy.MyMethod3))!,
				]).InAnyOrder();
				await That(methods.GetDescription())
					.IsEqualTo(
						"methods with parameter matching p => p.Position == 0 && p.ParameterType == typeof(int) " +
						"or with parameter matching p => p.Position == 1 && p.ParameterType == typeof(string)")
					.AsPrefix();
			}
		}

		private class Dummy
		{
			public void MyMethod0()
			{
			}

			public void MyMethod1(int value)
			{
				_ = value;
			}

			public void MyMethod2(string value)
			{
				_ = value;
			}

			public void MyMethod3(int first, string second)
			{
				_ = first;
				_ = second;
			}
		}
	}
}
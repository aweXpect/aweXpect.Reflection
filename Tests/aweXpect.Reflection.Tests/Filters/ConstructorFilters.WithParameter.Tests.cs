using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ConstructorFilters
{
	public sealed class WithParameter
	{
		public sealed class ParameterTests
		{
			[Fact]
			public async Task ShouldFilterForConstructorsWithParameterMatchingPredicate()
			{
				Filtered.Constructors constructors = In.Type<Dummy>()
					.Constructors().WithParameter(p => p.Name == "value");

				await That(constructors).IsEqualTo([
					typeof(Dummy).GetConstructor([typeof(int)])!,
					typeof(Dummy).GetConstructor([typeof(string)])!,
				]).InAnyOrder();
				await That(constructors.GetDescription())
					.IsEqualTo("constructors with parameter matching p => p.Name == \"value\"")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForConstructorsWithParameterOfSpecificType()
			{
				Filtered.Constructors constructors = In.Type<Dummy>()
					.Constructors().WithParameter(p => p.ParameterType == typeof(string));

				await That(constructors).IsEqualTo([
					typeof(Dummy).GetConstructor([typeof(string)])!,
					typeof(Dummy).GetConstructor([typeof(int), typeof(string)])!,
				]).InAnyOrder();
				await That(constructors.GetDescription())
					.IsEqualTo("constructors with parameter matching p => p.ParameterType == typeof(string)")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForConstructorsWithParameterAtSpecificPosition()
			{
				Filtered.Constructors constructors = In.Type<Dummy>()
					.Constructors().WithParameter(p => p.Position == 0);

				await That(constructors).IsEqualTo([
					typeof(Dummy).GetConstructor([typeof(int)])!,
					typeof(Dummy).GetConstructor([typeof(string)])!,
					typeof(Dummy).GetConstructor([typeof(int), typeof(string)])!,
				]).InAnyOrder();
				await That(constructors.GetDescription())
					.IsEqualTo("constructors with parameter matching p => p.Position == 0")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForConstructorsWithNoParameters()
			{
				Filtered.Constructors constructors = In.Type<Dummy>()
					.Constructors().WithParameter(p => false);

				await That(constructors).IsEmpty();
				await That(constructors.GetDescription())
					.IsEqualTo("constructors with parameter matching p => false")
					.AsPrefix();
			}
		}

		public sealed class OrWithParameterTests
		{
			[Fact]
			public async Task ShouldFilterForConstructorsWithParameterMatchingEitherPredicate()
			{
				Filtered.Constructors constructors = In.Type<Dummy>()
					.Constructors().WithParameter(p => p.ParameterType == typeof(int))
					.OrWithParameter(p => p.ParameterType == typeof(string));

				await That(constructors).IsEqualTo([
					typeof(Dummy).GetConstructor([typeof(int)])!,
					typeof(Dummy).GetConstructor([typeof(string)])!,
					typeof(Dummy).GetConstructor([typeof(int), typeof(string)])!,
				]).InAnyOrder();
				await That(constructors.GetDescription())
					.IsEqualTo(
						"constructors with parameter matching p => p.ParameterType == typeof(int) " +
						"or with parameter matching p => p.ParameterType == typeof(string)")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForConstructorsWithParameterAtDifferentPositions()
			{
				Filtered.Constructors constructors = In.Type<Dummy>()
					.Constructors().WithParameter(p => p.Position == 0 && p.ParameterType == typeof(int))
					.OrWithParameter(p => p.Position == 1 && p.ParameterType == typeof(string));

				await That(constructors).IsEqualTo([
					typeof(Dummy).GetConstructor([typeof(int)])!,
					typeof(Dummy).GetConstructor([typeof(int), typeof(string)])!,
				]).InAnyOrder();
				await That(constructors.GetDescription())
					.IsEqualTo(
						"constructors with parameter matching p => p.Position == 0 && p.ParameterType == typeof(int) " +
						"or with parameter matching p => p.Position == 1 && p.ParameterType == typeof(string)")
					.AsPrefix();
			}
		}

		private class Dummy
		{
			public Dummy()
			{
			}

			public Dummy(int value)
			{
				_ = value;
			}

			public Dummy(string value)
			{
				_ = value;
			}

			public Dummy(int first, string second)
			{
				_ = first;
				_ = second;
			}
		}
	}
}
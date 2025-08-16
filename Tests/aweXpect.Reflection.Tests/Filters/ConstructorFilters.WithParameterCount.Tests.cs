using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class ConstructorFilters
{
	public sealed class WithoutParameters
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterForConstructorsWithoutParameters()
			{
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithoutParameters();

				await That(constructors).Contains(ExpectedParameterlessConstructorInfo());
				await That(constructors.GetDescription())
					.IsEqualTo("constructors without parameters in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldNotIncludeConstructorsWithParameters()
			{
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithoutParameters();

				await That(constructors).DoesNotContain(ExpectedSingleParameterConstructorInfo());
				await That(constructors).DoesNotContain(ExpectedMultipleParameterConstructorInfo());
			}

			[Fact]
			public async Task ShouldBeEquivalentToWithParameterCountZero()
			{
				Filtered.Constructors constructorsWithoutParameters = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithoutParameters();
				Filtered.Constructors constructorsWithParameterCountZero = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithParameterCount(0);

				await That(constructorsWithoutParameters.ToArray())
					.IsEquivalentTo(constructorsWithParameterCountZero.ToArray());
			}
		}
	}

	public sealed class WithParameterCount
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterForConstructorsWithSpecifiedParameterCount()
			{
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithParameterCount(1);

				await That(constructors).Contains(ExpectedSingleParameterConstructorInfo());
				await That(constructors.GetDescription())
					.IsEqualTo("constructors with 1 parameter in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForConstructorsWithMultipleParameters()
			{
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithParameterCount(2);

				await That(constructors).Contains(ExpectedMultipleParameterConstructorInfo());
				await That(constructors.GetDescription())
					.IsEqualTo("constructors with 2 parameters in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForConstructorsWithZeroParameters()
			{
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithParameterCount(0);

				await That(constructors).Contains(ExpectedParameterlessConstructorInfo());
				await That(constructors.GetDescription())
					.IsEqualTo("constructors without parameters in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldNotIncludeConstructorsWithDifferentParameterCount()
			{
				Filtered.Constructors constructors = In.AssemblyContaining<AssemblyFilters>()
					.Constructors().WithParameterCount(1);

				await That(constructors).DoesNotContain(ExpectedParameterlessConstructorInfo());
				await That(constructors).DoesNotContain(ExpectedMultipleParameterConstructorInfo());
			}
		}
	}

	private static ConstructorInfo? ExpectedParameterlessConstructorInfo()
		=> typeof(ClassWithMultipleConstructors)
			.GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, 
				null, new System.Type[0], null);

	private static ConstructorInfo? ExpectedSingleParameterConstructorInfo()
		=> typeof(ClassWithMultipleConstructors)
			.GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				null, new[] { typeof(int) }, null);

	private static ConstructorInfo? ExpectedMultipleParameterConstructorInfo()
		=> typeof(ClassWithMultipleConstructors)
			.GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				null, new[] { typeof(string), typeof(int) }, null);

	private class ClassWithMultipleConstructors
	{
		public ClassWithMultipleConstructors()
		{
		}

		public ClassWithMultipleConstructors(int value)
		{
		}

		public ClassWithMultipleConstructors(string name, int value)
		{
		}
	}
}
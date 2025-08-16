using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class MethodFilters
{
	public sealed class WithoutParameters
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterForMethodsWithoutParameters()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithoutParameters();

				await That(methods).Contains(ExpectedParameterlessMethodInfo());
				await That(methods.GetDescription())
					.IsEqualTo("methods without parameters in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldNotIncludeMethodsWithParameters()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithoutParameters();

				await That(methods).DoesNotContain(ExpectedSingleParameterMethodInfo());
				await That(methods).DoesNotContain(ExpectedMultipleParameterMethodInfo());
			}

			[Fact]
			public async Task ShouldBeEquivalentToWithParameterCountZero()
			{
				Filtered.Methods methodsWithoutParameters = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithoutParameters();
				Filtered.Methods methodsWithParameterCountZero = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithParameterCount(0);

				await That(methodsWithoutParameters.ToArray())
					.IsEquivalentTo(methodsWithParameterCountZero.ToArray());
			}
		}
	}

	public sealed class WithParameterCount
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterForMethodsWithSpecifiedParameterCount()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithParameterCount(1);

				await That(methods).Contains(ExpectedSingleParameterMethodInfo());
				await That(methods.GetDescription())
					.IsEqualTo("methods with 1 parameter in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForMethodsWithMultipleParameters()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithParameterCount(2);

				await That(methods).Contains(ExpectedMultipleParameterMethodInfo());
				await That(methods.GetDescription())
					.IsEqualTo("methods with 2 parameters in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldFilterForMethodsWithZeroParameters()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithParameterCount(0);

				await That(methods).Contains(ExpectedParameterlessMethodInfo());
				await That(methods.GetDescription())
					.IsEqualTo("methods without parameters in assembly")
					.AsPrefix();
			}

			[Fact]
			public async Task ShouldNotIncludeMethodsWithDifferentParameterCount()
			{
				Filtered.Methods methods = In.AssemblyContaining<AssemblyFilters>()
					.Methods().WithParameterCount(1);

				await That(methods).DoesNotContain(ExpectedParameterlessMethodInfo());
				await That(methods).DoesNotContain(ExpectedMultipleParameterMethodInfo());
			}
		}
	}

	private static MethodInfo? ExpectedParameterlessMethodInfo()
		=> typeof(ClassWithMultipleMethods)
			.GetMethod(nameof(ClassWithMultipleMethods.MethodWithoutParameters));

	private static MethodInfo? ExpectedSingleParameterMethodInfo()
		=> typeof(ClassWithMultipleMethods)
			.GetMethod(nameof(ClassWithMultipleMethods.MethodWithSingleParameter));

	private static MethodInfo? ExpectedMultipleParameterMethodInfo()
		=> typeof(ClassWithMultipleMethods)
			.GetMethod(nameof(ClassWithMultipleMethods.MethodWithMultipleParameters));

	private class ClassWithMultipleMethods
	{
#pragma warning disable CA1822
		public void MethodWithoutParameters()
		{
		}

		public void MethodWithSingleParameter(int value)
		{
		}

		public void MethodWithMultipleParameters(string name, int value)
		{
		}
#pragma warning restore CA1822
	}
}
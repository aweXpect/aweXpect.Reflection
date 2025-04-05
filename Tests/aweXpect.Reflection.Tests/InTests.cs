using System.Collections.Generic;
using System.Reflection;

namespace aweXpect.Reflection.Tests;

// ReSharper disable PossibleMultipleEnumeration
public class InTests
{
	[Fact]
	public async Task AllLoadedAssemblies_ShouldContainMultipleAssemblies()
	{
		FilteredAssemblies sut = In.AllLoadedAssemblies();

		await That(sut).HasCount().Between(5).And(7);
		await That(sut).All().Satisfy(x => !x.FullName!.StartsWith("System."));
	}

	[Fact]
	public async Task AllLoadedAssemblies_WithoutInclusionFilter()
	{
		FilteredAssemblies sut = In.AllLoadedAssemblies(applyExclusionFilters: false);

		await That(sut).HasCount().AtLeast(8);
		await That(sut).AtLeast(1).Satisfy(x => x.FullName!.StartsWith("System."));
	}

	[Fact]
	public async Task AllLoadedAssemblies_WithPredicate_ShouldApplyPredicate()
	{
		FilteredAssemblies sut = In.AllLoadedAssemblies(assembly => !assembly.FullName!.StartsWith("aweXpect."));

		await That(sut).HasCount().Between(2).And(4);
	}

	[Fact]
	public async Task Assemblies_WithArray_ShouldContainProvidedAssemblies()
	{
		Assembly[] assemblies = [typeof(In).Assembly, typeof(InTests).Assembly,];

		FilteredAssemblies sut = In.Assemblies(assemblies);

		await That(sut).IsEqualTo(assemblies);
	}

	[Fact]
	public async Task Assemblies_WithEnumerable_ShouldContainProvidedAssemblies()
	{
		IEnumerable<Assembly> assemblies = [typeof(In).Assembly, typeof(InTests).Assembly,];

		FilteredAssemblies sut = In.Assemblies(assemblies);

		await That(sut).IsEqualTo(assemblies);
	}

	[Fact]
	public async Task AssemblyContaining_WithGeneric_ShouldContainAssemblyOfProvidedType()
	{
		FilteredAssemblies sut = In.AssemblyContaining<InTests>();

		await That(sut).HasSingle().Which
			.IsEqualTo(typeof(InTests).Assembly);
	}

	[Fact]
	public async Task AssemblyContaining_WithType_ShouldContainAssemblyOfProvidedType()
	{
		FilteredAssemblies sut = In.AssemblyContaining(typeof(In));

		await That(sut).HasSingle().Which
			.IsEqualTo(typeof(In).Assembly);
	}

	[Fact]
	public async Task EntryAssembly_ShouldContainExpectedAssembly()
	{
		Assembly? expectedAssembly = Assembly.GetEntryAssembly();

		FilteredAssemblies sut = In.EntryAssembly();

#if NET8_0_OR_GREATER
		await That(sut).HasSingle().Which
			.IsEqualTo(expectedAssembly);
#else
		await That(sut).IsEmpty();
#endif
	}

	[Fact]
	public async Task ExecutingAssembly_ShouldContainExpectedAssembly()
	{
		Assembly expectedAssembly = typeof(In).Assembly;

		FilteredAssemblies sut = In.ExecutingAssembly();

		await That(sut).HasSingle().Which
			.IsEqualTo(expectedAssembly);
	}
}

﻿using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Tests.TestHelpers;

namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class AssemblyFilters
{
	public sealed class WithName
	{
		public sealed class Tests
		{
			[Fact]
			public async Task ShouldFilterForAssembliesWithName()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("aweXpect.Reflection.Tests");

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo("in all loaded assemblies with name equal to \"aweXpect.Reflection.Tests\"");
			}

			[Fact]
			public async Task ShouldSupportAsPrefix()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("aweXpect.Reflection").AsPrefix();

				await That(Assemblies).HasCount().AtLeast(2);
				await That(Assemblies.GetDescription())
					.IsEqualTo("in all loaded assemblies with name starting with \"aweXpect.Reflection\"");
			}

			[Fact]
			public async Task ShouldSupportAsRegex()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("aweXpect\\.[a-zA-Z\\.]*").AsRegex();

				await That(Assemblies).HasCount().AtLeast(3);
				await That(Assemblies.GetDescription())
					.IsEqualTo("in all loaded assemblies with name matching regex \"aweXpect\\.[a-zA-Z\\.]*\"");
			}

			[Fact]
			public async Task ShouldSupportAsSuffix()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName(".Tests").AsSuffix();

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo("in all loaded assemblies with name ending with \".Tests\"");
			}

			[Fact]
			public async Task ShouldSupportAsWildcard()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("awe*pect*").AsWildcard();

				await That(Assemblies).HasCount().AtLeast(3);
				await That(Assemblies.GetDescription())
					.IsEqualTo("in all loaded assemblies with name matching \"awe*pect*\"");
			}

			[Fact]
			public async Task ShouldSupportExactly()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("aweXpect.Reflection.Tests").Exactly();

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo("in all loaded assemblies with name equal to \"aweXpect.Reflection.Tests\"");
			}

			[Fact]
			public async Task ShouldSupportIgnoringCase()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("awexpect.reflection.tests").IgnoringCase();

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo(
						"in all loaded assemblies with name equal to \"awexpect.reflection.tests\" ignoring case");
			}

			[Fact]
			public async Task ShouldSupportIgnoringLeadingWhiteSpace()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("\t aweXpect.Reflection.Tests")
					.IgnoringLeadingWhiteSpace();

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo(
						"in all loaded assemblies with name equal to \"\\t aweXpect.Reflection.Tests\" ignoring leading white-space");
			}

			[Fact]
			public async Task ShouldSupportIgnoringTrailingWhiteSpace()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("aweXpect.Reflection.Tests\t ")
					.IgnoringTrailingWhiteSpace();

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo(
						"in all loaded assemblies with name equal to \"aweXpect.Reflection.Tests\\t \" ignoring trailing white-space");
			}

			[Fact]
			public async Task ShouldSupportUsingCustomComparer()
			{
				Filtered.Assemblies Assemblies = In.AllLoadedAssemblies()
					.WithName("AwEXpEct.REflEctIOn.TEsts")
					.Using(new IgnoreCaseForVocalsComparer());

				await That(Assemblies).HasSingle().Which.IsEqualTo(typeof(AssemblyFilters).Assembly);
				await That(Assemblies.GetDescription())
					.IsEqualTo(
						"in all loaded assemblies with name equal to \"AwEXpEct.REflEctIOn.TEsts\" using IgnoreCaseForVocalsComparer");
			}
		}
	}
}

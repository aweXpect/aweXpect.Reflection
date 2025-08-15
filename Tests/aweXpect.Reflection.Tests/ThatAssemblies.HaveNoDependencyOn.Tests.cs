using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Tests.TestHelpers.Types;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssemblies
{
	public sealed class HaveNoDependencyOn
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssembliesHaveNoDependency_ShouldSucceed()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveNoDependencyOn("NonExistentAssembly");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAssembliesHaveDependency_ShouldFail()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveNoDependencyOn("System.Runtime");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that in assembly containing type PublicAbstractClass
					             all have no dependency on equal to "System.Runtime",
					             but it contained assemblies with dependency [
					               aweXpect.Reflection.Tests, Version=*, Culture=neutral, PublicKeyToken=null
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenNoDependencyMatchesIgnoringCase_ShouldFail()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveNoDependencyOn("system.runtime").IgnoringCase();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that in assembly containing type PublicAbstractClass
					             all have no dependency on equal to "system.runtime" (ignoring case),
					             but it contained assemblies with dependency [
					               aweXpect.Reflection.Tests, Version=*, Culture=neutral, PublicKeyToken=null
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenNoDependencyMatchesAsPrefix_ShouldFail()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveNoDependencyOn("System").AsPrefix();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that in assembly containing type PublicAbstractClass
					             all have no dependency on starting with "System",
					             but it contained assemblies with dependency [
					               aweXpect.Reflection.Tests, Version=*, Culture=neutral, PublicKeyToken=null
					             ]
					             """).AsWildcard();
			}
		}
	}
}
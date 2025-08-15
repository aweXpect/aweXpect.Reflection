using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Tests.TestHelpers.Types;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssemblies
{
	public sealed class HaveDependencyOn
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssembliesHaveDependency_ShouldSucceed()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveDependencyOn("System.Runtime");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAssembliesDoNotHaveDependency_ShouldFail()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveDependencyOn("NonExistentAssembly");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that in assembly containing type PublicAbstractClass
					             all have dependency on equal to "NonExistentAssembly",
					             but it contained assemblies without dependency [
					               aweXpect.Reflection.Tests, Version=*, Culture=neutral, PublicKeyToken=null
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenDependencyMatchesIgnoringCase_ShouldSucceed()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveDependencyOn("system.runtime").IgnoringCase();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenDependencyMatchesAsPrefix_ShouldSucceed()
			{
				Filtered.Assemblies subject = In.AssemblyContaining<PublicAbstractClass>();

				async Task Act()
					=> await That(subject).HaveDependencyOn("System").AsPrefix();

				await That(Act).DoesNotThrow();
			}
		}
	}
}
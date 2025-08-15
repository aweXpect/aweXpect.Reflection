using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssembly
{
	public sealed class HasDependencyOn
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssemblyHasDependency_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("System.Runtime");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAssemblyDoesNotHaveDependency_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("NonExistentAssembly");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has dependency on equal to "NonExistentAssembly",
					             but it was "*" which differs at index 0:
					                ↓ (actual)
					               "*"
					               "NonExistentAssembly"
					                ↑ (expected)
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenAssemblyIsNull_ShouldFail()
			{
				Assembly? subject = null;

				async Task Act()
					=> await That(subject).HasDependencyOn("System.Runtime");

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             has dependency on equal to "System.Runtime",
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenDependencyMatchesIgnoringCase_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("system.runtime").IgnoringCase();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenDependencyMatchesAsPrefix_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("System").AsPrefix();

				await That(Act).DoesNotThrow();
			}
		}
	}
}
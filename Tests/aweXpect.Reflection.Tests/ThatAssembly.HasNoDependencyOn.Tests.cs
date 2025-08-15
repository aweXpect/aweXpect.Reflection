using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssembly
{
	public sealed class HasNoDependencyOn
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssemblyDoesNotHaveDependency_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasNoDependencyOn("NonExistentAssembly");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAssemblyHasDependency_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasNoDependencyOn("System.Runtime");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has no dependency on equal to "System.Runtime",
					             but it was "*" with a length of * which is longer than the expected length of 14 and has superfluous:
					               "*"
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenAssemblyIsNull_ShouldFail()
			{
				Assembly? subject = null;

				async Task Act()
					=> await That(subject).HasNoDependencyOn("System.Runtime");

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             has no dependency on equal to "System.Runtime",
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenNoDependencyMatchesIgnoringCase_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasNoDependencyOn("system.runtime").IgnoringCase();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has no dependency on equal to "system.runtime" ignoring case,
					             but it was "*" with a length of * which is longer than the expected length of 14 and has superfluous:
					               "*"
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenNoDependencyMatchesAsPrefix_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasNoDependencyOn("System").AsPrefix();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has no dependency on starting with "System",
					             but it was "*" which differs at index 6:
					                      ↓ (actual)
					               "System.*"
					               "System"
					                      ↑ (expected)
					             """).AsWildcard();
			}
		}
	}
}
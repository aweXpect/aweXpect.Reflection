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
			public async Task WhenAssemblyDoesNotHaveDependency_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("NonExistentAssembly");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             has dependency on assembly equal to "NonExistentAssembly",
					             but it had the required dependencies [*]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenAssemblyHasDependency_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("System.Runtime");

				await That(Act).DoesNotThrow();
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
					             has dependency on assembly equal to "System.Runtime",
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenDependencyMatchesAsPrefix_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("System").AsPrefix();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenDependencyMatchesIgnoringCase_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).HasDependencyOn("system.runtime").IgnoringCase();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenAssemblyDoesNotHaveDependency_ShouldSucceed()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).DoesNotComplyWith(it => it.HasDependencyOn("NonExistentAssembly"));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAssemblyHasDependency_ShouldFail()
			{
				Assembly subject = typeof(PublicAbstractClass).Assembly;

				async Task Act()
					=> await That(subject).DoesNotComplyWith(it => it.HasDependencyOn("System.Runtime"));

				await That(Act).Throws<XunitException>()
					.WithMessage("*does not have dependency on assembly*")
					.AsWildcard();
			}
		}
	}
}

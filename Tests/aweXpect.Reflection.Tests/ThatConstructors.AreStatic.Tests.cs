using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Tests.TestHelpers.Types;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatConstructors
{
	public sealed class AreStatic
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenConstructorsContainNonStaticConstructors_ShouldFail()
			{
				IEnumerable<ConstructorInfo> subject = typeof(TestClassWithStaticMembers)
					.GetConstructors(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

				async Task Act()
					=> await That(subject).AreStatic();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             are all static,
					             but it contained non-static constructors [
					               *
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenFilteringOnlyStaticConstructors_ShouldSucceed()
			{
				IEnumerable<ConstructorInfo> subject = typeof(TestClassWithStaticMembers)
					.GetConstructors(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

				async Task Act()
					=> await That(subject).AreStatic();

				await That(Act).DoesNotThrow();
			}
		}
	}
}
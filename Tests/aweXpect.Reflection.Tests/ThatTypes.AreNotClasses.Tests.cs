﻿using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatTypes
{
	public sealed class AreNotClasses
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenAssembliesContainOnlyInterfaceTypes_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<AreNotClasses>().Interfaces();

				async Task Act()
					=> await That(subject).AreNotClasses();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenFilteringOnlyClasses_ShouldFail()
			{
				Filtered.Types subject = In.AssemblyContaining<AreNotClasses>().Types()
					.WhichSatisfy(type => type.IsClass);

				async Task Act()
					=> await That(subject).AreNotClasses();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that types matching type => type.IsClass in assembly containing type ThatTypes.AreNotClasses
					             are all not classes,
					             but it contained classes [
					               *
					             ]
					             """).AsWildcard();
			}
		}
	}
}

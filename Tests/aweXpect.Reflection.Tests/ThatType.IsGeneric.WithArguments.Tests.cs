using System;
using System.Collections.Generic;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsGenericWithArguments
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WithCount_WhenTypeHasCorrectGenericArgumentCount_ShouldSucceed()
			{
				Type subject = typeof(Dictionary<string, int>);

				async Task Act()
					=> await That(subject).IsGeneric().WithCount(2);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WithCount_WhenTypeHasIncorrectGenericArgumentCount_ShouldFail()
			{
				Type subject = typeof(List<string>);

				async Task Act()
					=> await That(subject).IsGeneric().WithCount(2);

				await That(Act).Throws<XunitException>();
			}

			[Fact]
			public async Task AtIndex_WhenCheckingGenericArgumentAtIndex_ShouldSucceed()
			{
				Type subject = typeof(Dictionary<string, int>);

				async Task Act()
					=> await That(subject).IsGeneric().AtIndex(1);

				await That(Act).DoesNotThrow();
			}
		}
	}
}
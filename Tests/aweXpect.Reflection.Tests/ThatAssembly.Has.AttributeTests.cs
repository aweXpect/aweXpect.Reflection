using System;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssembly
{
	public sealed class Has
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenAssemblyHasAttribute_ShouldSucceed()
			{
				Assembly subject = Assembly.GetExecutingAssembly();

				async Task Act()
					=> await That(subject).Has<AssemblyTitleAttribute>();

				await That(Act).DoesNotThrow();
			}

			[AttributeUsage(AttributeTargets.Assembly)]
			private class TestAttribute : Attribute
			{
			}
		}
	}
}
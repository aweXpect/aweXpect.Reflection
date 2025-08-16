using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatAssemblies
{
	public sealed class Have
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenAllAssembliesHaveAttribute_ShouldSucceed()
			{
				IEnumerable<Assembly> subject = new[]
				{
					typeof(AttributeTests).Assembly
				};

				async Task Act()
					=> await That(subject).Have<AssemblyTitleAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllAssembliesHaveMatchingAttribute_ShouldSucceed()
			{
				IEnumerable<Assembly> subject = new[]
				{
					typeof(AttributeTests).Assembly
				};

				async Task Act()
					=> await That(subject).Have<AssemblyTitleAttribute>(attr => !string.IsNullOrEmpty(attr.Title));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenNotAllAssembliesHaveMatchingAttribute_ShouldFail()
			{
				IEnumerable<Assembly> subject = new[]
				{
					typeof(AttributeTests).Assembly
				};

				async Task Act()
					=> await That(subject).Have<AssemblyTitleAttribute>(attr => attr.Title == "NonExistentTitle");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have AssemblyTitleAttribute matching attr => attr.Title == "NonExistentTitle",
					             but it contained not matching assemblies [
					               aweXpect.Reflection.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
					             ]
					             """);
			}
		}
	}
}
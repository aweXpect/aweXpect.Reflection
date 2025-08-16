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
				IEnumerable<Assembly> subjects = new[]
				{
					typeof(AttributeTests).Assembly,
				};

				async Task Act()
					=> await That(subjects).Have<AssemblyTitleAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllAssembliesHaveMatchingAttribute_ShouldSucceed()
			{
				IEnumerable<Assembly> subjects = new[]
				{
					typeof(AttributeTests).Assembly,
				};

				async Task Act()
					=> await That(subjects)
						.Have<AssemblyTitleAttribute>(attr => attr.Title == "aweXpect.Reflection.Tests");

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenNotAllAssembliesHaveAttribute_ShouldFail()
			{
				IEnumerable<Assembly> subjects = new[]
				{
					typeof(AttributeTests).Assembly,
				};

				async Task Act()
					=> await That(subjects).Have<TestAttribute>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subjects
					             all have ThatAssemblies.Have.AttributeTests.TestAttribute,
					             but it contained not matching assemblies [
					               aweXpect.Reflection.Tests, Version=*, Culture=neutral, PublicKeyToken=null
					             ]
					             """).AsWildcard();
			}

			[Fact]
			public async Task WhenNotAllAssembliesHaveMatchingAttribute_ShouldFail()
			{
				IEnumerable<Assembly> subjects = new[]
				{
					typeof(AttributeTests).Assembly,
				};

				async Task Act()
					=> await That(subjects).Have<AssemblyTitleAttribute>(attr => attr.Title == "NonExistentTitle");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subjects
					             all have AssemblyTitleAttribute matching attr => attr.Title == "NonExistentTitle",
					             but it contained not matching assemblies [
					               aweXpect.Reflection.Tests, Version=*, Culture=neutral, PublicKeyToken=null
					             ]
					             """).AsWildcard();
			}

			[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
			private class TestAttribute : Attribute;
		}
	}
}

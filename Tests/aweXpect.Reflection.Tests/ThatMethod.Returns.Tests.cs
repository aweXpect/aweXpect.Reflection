using System;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
	public sealed class Returns
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenMethodReturnsExpectedType_ShouldSucceed()
			{
				MethodInfo subject = GetMethod("PublicMethod")!;

				await That(subject).Returns<int>();
			}

			[Fact]
			public async Task WhenMethodReturnsExpectedTypeNonGeneric_ShouldSucceed()
			{
				MethodInfo subject = GetMethod("PublicMethod")!;

				await That(subject).Returns(typeof(int));
			}

			[Fact]
			public async Task WhenMethodDoesNotReturnExpectedType_ShouldFail()
			{
				MethodInfo subject = GetMethod("PublicMethod")!;

				async Task Act()
					=> await That(subject).Returns<string>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             returns System.String,
					             but it returned System.Int32
					             """);
			}

			[Fact]
			public async Task WhenMethodInfoIsNull_ShouldFail()
			{
				MethodInfo? subject = null;

				async Task Act()
					=> await That(subject).Returns<int>();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             returns System.Int32,
					             but it was <null>
					             """);
			}

			[Fact]
			public async Task WhenReturnTypeInheritsFromExpectedType_ShouldSucceed()
			{
				MethodInfo subject = typeof(ClassWithInheritance).GetMethod("GetDerived")!;

				await That(subject).Returns<BaseClass>();
			}

			[Fact]
			public async Task WithOrReturns_WhenMethodReturnsOneOfTheTypes_ShouldSucceed()
			{
				MethodInfo subject = GetMethod("PublicMethod")!;

				await That(subject).Returns<string>().OrReturns<int>();
			}

			[Fact]
			public async Task WithOrReturns_WhenMethodReturnsNoneOfTheTypes_ShouldFail()
			{
				MethodInfo subject = GetMethod("PublicMethod")!;

				async Task Act()
					=> await That(subject).Returns<string>().OrReturns<bool>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             returns System.String or returns System.Boolean,
					             but it returned System.Int32
					             """);
			}

			[Fact]
			public async Task WithMultipleOrReturns_ShouldSupportChaining()
			{
				MethodInfo subject = GetMethod("PublicMethod")!;

				await That(subject).Returns<string>().OrReturns<bool>().OrReturns<int>();
			}

#pragma warning disable CA1822 // Mark members as static
			public class ClassWithInheritance
			{
				public DerivedClass GetDerived() => new();
			}

			public abstract class BaseClass;
			public class DerivedClass : BaseClass;
#pragma warning restore CA1822 // Mark members as static
		}
	}
}
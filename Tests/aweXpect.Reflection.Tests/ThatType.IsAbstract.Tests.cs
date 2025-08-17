﻿using aweXpect.Reflection.Tests.TestHelpers.Types;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsAbstract
	{
		public sealed class Tests
		{
			[Fact]
			public async Task WhenTypeIsAbstract_ShouldSucceed()
			{
				Type subject = typeof(PublicAbstractClass);

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).DoesNotThrow();
			}

			[Theory]
			[MemberData(nameof(NonAbstractTypes))]
			public async Task WhenTypeIsNotAbstract_ShouldFail(Type subject)
			{
				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).ThrowsException()
					.WithMessage($"""
					              Expected that subject
					              is abstract,
					              but it was non-abstract {Formatter.Format(subject)}
					              """);
			}

			[Fact]
			public async Task WhenTypeIsNull_ShouldFail()
			{
				Type? subject = null;

				async Task Act()
					=> await That(subject).IsAbstract();

				await That(Act).ThrowsException()
					.WithMessage("""
					             Expected that subject
					             is abstract,
					             but it was <null>
					             """);
			}

			public static TheoryData<Type> NonAbstractTypes()
				=>
				[
					typeof(PublicStaticClass),
					typeof(PublicSealedClass),
					typeof(Container.PublicNestedClass),
					typeof(IPublicInterface),
				];
		}

		public sealed class NegatedTests
		{
			[Theory]
			[MemberData(nameof(NonAbstractTypesForNegated))]
			public async Task WhenTypeIsNotAbstract_ShouldSucceed(Type subject)
			{
				async Task Act()
					=> await That(subject).DoesNotComplyWith(it => it.IsAbstract());

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeIsAbstract_ShouldFail()
			{
				Type subject = typeof(PublicAbstractClass);

				async Task Act()
					=> await That(subject).DoesNotComplyWith(it => it.IsAbstract());

				await That(Act).Throws<XunitException>()
					.WithMessage($"""
					              Expected that subject
					              is not abstract,
					              but it was abstract {Formatter.Format(subject)}
					              """);
			}

			public static TheoryData<Type> NonAbstractTypesForNegated() => new()
			{
				typeof(PublicStaticClass),
				typeof(PublicSealedClass),
				typeof(Container.PublicNestedClass),
				typeof(IPublicInterface),
			};

			public static TheoryData<Type> NonAbstractTypes()
				=>
				[
					typeof(PublicStaticClass),
					typeof(PublicSealedClass),
					typeof(Container.PublicNestedClass),
					typeof(IPublicInterface),
				];
		}
	}
}

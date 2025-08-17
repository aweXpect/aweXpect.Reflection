using System;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class InheritsFrom
	{
		public sealed class GenericTests
		{
			[Fact]
			public async Task WhenTypeInheritsFromBaseClass_ShouldSucceed()
			{
				Type subject = typeof(DerivedClass);

				async Task Act()
					=> await That(subject).InheritsFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeImplementsInterface_ShouldSucceed()
			{
				Type subject = typeof(ClassWithInterface);

				async Task Act()
					=> await That(subject).InheritsFrom<ITestInterface>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeDoesNotInherit_ShouldFail()
			{
				Type subject = typeof(UnrelatedClass);

				async Task Act()
					=> await That(subject).InheritsFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             inherits from ThatType.InheritsFrom.GenericTests.BaseClass,
					             but it did not inherit from ThatType.InheritsFrom.GenericTests.BaseClass, but was ThatType.InheritsFrom.GenericTests.UnrelatedClass
					             """);
			}

			[Fact]
			public async Task WhenTypeIsSameAsBaseType_ShouldFail()
			{
				Type subject = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritsFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             inherits from ThatType.InheritsFrom.GenericTests.BaseClass,
					             but it did not inherit from ThatType.InheritsFrom.GenericTests.BaseClass, but was ThatType.InheritsFrom.GenericTests.BaseClass
					             """);
			}

			[Fact]
			public async Task WhenTypeInheritsIndirectly_ShouldSucceed()
			{
				Type subject = typeof(GrandChildClass);

				async Task Act()
					=> await That(subject).InheritsFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeInheritsIndirectly_WithForceDirect_ShouldFail()
			{
				Type subject = typeof(GrandChildClass);

				async Task Act()
					=> await That(subject).InheritsFrom<BaseClass>(forceDirect: true);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             inherits from directly ThatType.InheritsFrom.GenericTests.BaseClass,
					             but it did not inherit from directly ThatType.InheritsFrom.GenericTests.BaseClass, but was ThatType.InheritsFrom.GenericTests.GrandChildClass
					             """);
			}

			[Fact]
			public async Task WhenTypeInheritsDirectly_WithForceDirect_ShouldSucceed()
			{
				Type subject = typeof(DerivedClass);

				async Task Act()
					=> await That(subject).InheritsFrom<BaseClass>(forceDirect: true);

				await That(Act).DoesNotThrow();
			}

			private class BaseClass;
			private class DerivedClass : BaseClass;
			private class GrandChildClass : DerivedClass;
			private class UnrelatedClass;
			private class ClassWithInterface : ITestInterface;
			private interface ITestInterface;
		}

		public sealed class TypeTests
		{
			[Fact]
			public async Task WhenTypeInheritsFromBaseClass_ShouldSucceed()
			{
				Type subject = typeof(DerivedClass);
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritsFrom(baseType);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeImplementsInterface_ShouldSucceed()
			{
				Type subject = typeof(ClassWithInterface);
				Type interfaceType = typeof(ITestInterface);

				async Task Act()
					=> await That(subject).InheritsFrom(interfaceType);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeDoesNotInherit_ShouldFail()
			{
				Type subject = typeof(UnrelatedClass);
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritsFrom(baseType);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             inherits from ThatType.InheritsFrom.TypeTests.BaseClass,
					             but it did not inherit from ThatType.InheritsFrom.TypeTests.BaseClass, but was ThatType.InheritsFrom.TypeTests.UnrelatedClass
					             """);
			}

			[Fact]
			public async Task WhenTypeInheritsIndirectly_WithForceDirect_ShouldFail()
			{
				Type subject = typeof(GrandChildClass);
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritsFrom(baseType, forceDirect: true);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             inherits from directly ThatType.InheritsFrom.TypeTests.BaseClass,
					             but it did not inherit from directly ThatType.InheritsFrom.TypeTests.BaseClass, but was ThatType.InheritsFrom.TypeTests.GrandChildClass
					             """);
			}

			[Fact]
			public async Task WhenTypeInheritsDirectly_WithForceDirect_ShouldSucceed()
			{
				Type subject = typeof(DerivedClass);
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritsFrom(baseType, forceDirect: true);

				await That(Act).DoesNotThrow();
			}

			private class BaseClass;
			private class DerivedClass : BaseClass;
			private class GrandChildClass : DerivedClass;
			private class UnrelatedClass;
			private class ClassWithInterface : ITestInterface;
			private interface ITestInterface;
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenTypeDoesNotInherit_ShouldSucceed()
			{
				Type subject = typeof(UnrelatedClass);

				async Task Act()
					=> await That(subject).DoesNotInheritFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypeInherits_ShouldFail()
			{
				Type subject = typeof(DerivedClass);

				async Task Act()
					=> await That(subject).DoesNotInheritFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             does not inherit from ThatType.InheritsFrom.NegatedTests.BaseClass,
					             but it did inherit from ThatType.InheritsFrom.NegatedTests.BaseClass
					             """);
			}

			private class BaseClass;
			private class DerivedClass : BaseClass;
			private class UnrelatedClass;
		}
	}
}
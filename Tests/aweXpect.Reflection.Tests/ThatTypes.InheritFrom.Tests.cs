using System;
using System.Collections.Generic;
using aweXpect.Reflection.Collections;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatTypes
{
	public sealed class InheritFrom
	{
		public sealed class GenericTests
		{
			[Fact]
			public async Task WhenAllTypesInheritFromBaseClass_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<GenericTests>()
					.Types()
					.Which(type => type == typeof(DerivedClass1) || type == typeof(DerivedClass2));

				async Task Act()
					=> await That(subject).InheritFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllTypesImplementInterface_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<GenericTests>()
					.Types()
					.Which(type => type == typeof(ClassWithInterface1) || type == typeof(ClassWithInterface2));

				async Task Act()
					=> await That(subject).InheritFrom<ITestInterface>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenSomeTypesDoNotInherit_ShouldFail()
			{
				Filtered.Types subject = In.AssemblyContaining<GenericTests>()
					.Types()
					.Which(type => type == typeof(DerivedClass1) || type == typeof(UnrelatedClass));

				async Task Act()
					=> await That(subject).InheritFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that types matching type => type == typeof(DerivedClass1) || type == typeof(UnrelatedClass) in assembly containing type ThatTypes.InheritFrom.GenericTests
					             all inherit from ThatTypes.InheritFrom.GenericTests.BaseClass,
					             but it contained types that do not inherit from ThatTypes.InheritFrom.GenericTests.BaseClass [
					               ThatTypes.InheritFrom.GenericTests.UnrelatedClass
					             ]
					             """);
			}

			[Fact]
			public async Task WhenTypesInheritIndirectly_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<GenericTests>()
					.Types()
					.Which(type => type == typeof(GrandChildClass));

				async Task Act()
					=> await That(subject).InheritFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenTypesInheritIndirectly_WithForceDirect_ShouldFail()
			{
				Filtered.Types subject = In.AssemblyContaining<GenericTests>()
					.Types()
					.Which(type => type == typeof(GrandChildClass));

				async Task Act()
					=> await That(subject).InheritFrom<BaseClass>(forceDirect: true);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that types matching type => type == typeof(GrandChildClass) in assembly containing type ThatTypes.InheritFrom.GenericTests
					             all inherit from directly ThatTypes.InheritFrom.GenericTests.BaseClass,
					             but it contained types that do not inherit from directly ThatTypes.InheritFrom.GenericTests.BaseClass [
					               ThatTypes.InheritFrom.GenericTests.GrandChildClass
					             ]
					             """);
			}

			[Fact]
			public async Task WhenTypesInheritDirectly_WithForceDirect_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<GenericTests>()
					.Types()
					.Which(type => type == typeof(DerivedClass1) || type == typeof(DerivedClass2));

				async Task Act()
					=> await That(subject).InheritFrom<BaseClass>(forceDirect: true);

				await That(Act).DoesNotThrow();
			}

			private class BaseClass;
			private class DerivedClass1 : BaseClass;
			private class DerivedClass2 : BaseClass;
			private class GrandChildClass : DerivedClass1;
			private class UnrelatedClass;
			private class ClassWithInterface1 : ITestInterface;
			private class ClassWithInterface2 : ITestInterface;
			private interface ITestInterface;
		}

		public sealed class TypeTests
		{
			[Fact]
			public async Task WhenAllTypesInheritFromBaseClass_ShouldSucceed()
			{
				IEnumerable<Type?> subject = new[] { typeof(DerivedClass1), typeof(DerivedClass2) };
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritFrom(baseType);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllTypesImplementInterface_ShouldSucceed()
			{
				IEnumerable<Type?> subject = new[] { typeof(ClassWithInterface1), typeof(ClassWithInterface2) };
				Type interfaceType = typeof(ITestInterface);

				async Task Act()
					=> await That(subject).InheritFrom(interfaceType);

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenSomeTypesDoNotInherit_ShouldFail()
			{
				IEnumerable<Type?> subject = new[] { typeof(DerivedClass1), typeof(UnrelatedClass) };
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritFrom(baseType);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all inherit from ThatTypes.InheritFrom.TypeTests.BaseClass,
					             but it contained types that do not inherit from ThatTypes.InheritFrom.TypeTests.BaseClass [
					               ThatTypes.InheritFrom.TypeTests.UnrelatedClass
					             ]
					             """);
			}

			[Fact]
			public async Task WhenTypesInheritIndirectly_WithForceDirect_ShouldFail()
			{
				IEnumerable<Type?> subject = new[] { typeof(GrandChildClass) };
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritFrom(baseType, forceDirect: true);

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all inherit from directly ThatTypes.InheritFrom.TypeTests.BaseClass,
					             but it contained types that do not inherit from directly ThatTypes.InheritFrom.TypeTests.BaseClass [
					               ThatTypes.InheritFrom.TypeTests.GrandChildClass
					             ]
					             """);
			}

			[Fact]
			public async Task WhenTypesInheritDirectly_WithForceDirect_ShouldSucceed()
			{
				IEnumerable<Type?> subject = new[] { typeof(DerivedClass1), typeof(DerivedClass2) };
				Type baseType = typeof(BaseClass);

				async Task Act()
					=> await That(subject).InheritFrom(baseType, forceDirect: true);

				await That(Act).DoesNotThrow();
			}

			private class BaseClass;
			private class DerivedClass1 : BaseClass;
			private class DerivedClass2 : BaseClass;
			private class GrandChildClass : DerivedClass1;
			private class UnrelatedClass;
			private class ClassWithInterface1 : ITestInterface;
			private class ClassWithInterface2 : ITestInterface;
			private interface ITestInterface;
		}

		public sealed class NegatedTests
		{
			[Fact]
			public async Task WhenNotAllTypesInherit_ShouldSucceed()
			{
				Filtered.Types subject = In.AssemblyContaining<NegatedTests>()
					.Types()
					.Which(type => type == typeof(DerivedClass) || type == typeof(UnrelatedClass));

				async Task Act()
					=> await That(subject).DoNotInheritFrom<BaseClass>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllTypesInherit_ShouldFail()
			{
				Filtered.Types subject = In.AssemblyContaining<NegatedTests>()
					.Types()
					.Which(type => type == typeof(DerivedClass1) || type == typeof(DerivedClass2));

				async Task Act()
					=> await That(subject).DoNotInheritFrom<BaseClass>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that types matching type => type == typeof(DerivedClass1) || type == typeof(DerivedClass2) in assembly containing type ThatTypes.InheritFrom.NegatedTests
					             not all inherit from ThatTypes.InheritFrom.NegatedTests.BaseClass,
					             but it only contained types that inherit from ThatTypes.InheritFrom.NegatedTests.BaseClass [
					               ThatTypes.InheritFrom.NegatedTests.DerivedClass1,
					               ThatTypes.InheritFrom.NegatedTests.DerivedClass2
					             ]
					             """);
			}

			private class BaseClass;
			private class DerivedClass : BaseClass;
			private class DerivedClass1 : BaseClass;
			private class DerivedClass2 : BaseClass;
			private class UnrelatedClass;
		}
	}
}
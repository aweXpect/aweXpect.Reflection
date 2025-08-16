using System;
using aweXpect.Reflection.Tests;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests
{
    public sealed partial class ThatType
    {
        public sealed class OrHave
        {
            public sealed class AttributeTests
            {
                [Fact]
                public async Task WhenTypeHasFirstAttribute_ShouldSucceed()
                {
                    Type subject = typeof(FooClass);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>().OrHave<BarAttribute>();

                    await That(Act).DoesNotThrow();
                }

                [Fact]
                public async Task WhenTypeHasSecondAttribute_ShouldSucceed()
                {
                    Type subject = typeof(BarClass);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>().OrHave<BarAttribute>();

                    await That(Act).DoesNotThrow();
                }

                [Fact]
                public async Task WhenTypeHasBothAttributes_ShouldSucceed()
                {
                    Type subject = typeof(FooBarClass);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>().OrHave<BarAttribute>();

                    await That(Act).DoesNotThrow();
                }

                [Fact]
                public async Task WhenTypeHasNeitherAttribute_ShouldFail()
                {
                    Type subject = typeof(BazClass);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>().OrHave<BarAttribute>();

                    await That(Act).Throws<XunitException>()
                        .WithMessage("""
                                     Expected that subject
                                     has ThatType.OrHave.AttributeTests.FooAttribute or has ThatType.OrHave.AttributeTests.BarAttribute,
                                     but it did not in ThatType.OrHave.AttributeTests.BazClass
                                     """);
                }

                [Fact]
                public async Task WhenTypeHasMatchingAttribute_ShouldSucceed()
                {
                    Type subject = typeof(FooClass2);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>(foo => foo.Value == 2).OrHave<BarAttribute>();

                    await That(Act).DoesNotThrow();
                }

                [Fact]
                public async Task WhenTypeHasMatchingSecondAttribute_ShouldSucceed()
                {
                    Type subject = typeof(BarClass3);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>(foo => foo.Value == 5).OrHave<BarAttribute>(bar => bar.Name == "test");

                    await That(Act).DoesNotThrow();
                }

                [Fact]
                public async Task WithPredicate_WhenTypeHasNotMatchingAttribute_ShouldFail()
                {
                    Type subject = typeof(FooClass2);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>(foo => foo.Value == 5).OrHave<BarAttribute>(bar => bar.Name == "test");

                    await That(Act).Throws<XunitException>()
                        .WithMessage("""
                                     Expected that subject
                                     has ThatType.OrHave.AttributeTests.FooAttribute matching foo => foo.Value == 5 or has ThatType.OrHave.AttributeTests.BarAttribute matching bar => bar.Name == "test",
                                     but it did not in ThatType.OrHave.AttributeTests.FooClass2
                                     """);
                }

                [Fact]
                public async Task WithInheritance_ShouldWorkCorrectly()
                {
                    Type subject = typeof(FooChildClass);

                    async Task Act()
                        => await That(subject).Has<FooAttribute>().OrHave<BarAttribute>();

                    await That(Act).DoesNotThrow();
                }

                [Fact]
                public async Task WithInheritanceFalse_ShouldWorkCorrectly()
                {
                    Type subject = typeof(FooChildClass);

                    async Task Act()
                        => await That(subject).Has<BazAttribute>(inherit: false).OrHave<BarAttribute>(inherit: false);

                    await That(Act).Throws<XunitException>()
                        .WithMessage("""
                                     Expected that subject
                                     has direct ThatType.OrHave.AttributeTests.BazAttribute or has direct ThatType.OrHave.AttributeTests.BarAttribute,
                                     but it did not in ThatType.OrHave.AttributeTests.FooChildClass
                                     """);
                }

                [AttributeUsage(AttributeTargets.Class)]
                private class FooAttribute : Attribute
                {
                    public int Value { get; set; }
                }

                [AttributeUsage(AttributeTargets.Class)]
                private class BarAttribute : Attribute
                {
                    public string? Name { get; set; }
                }

                [AttributeUsage(AttributeTargets.Class)]
                private class BazAttribute : Attribute
                {
                }

                [Foo]
                private class FooClass
                {
                }

                [Foo(Value = 2)]
                private class FooClass2
                {
                }

                [Bar]
                private class BarClass
                {
                }

                [Bar(Name = "test")]
                private class BarClass3
                {
                }

                [Foo]
                [Bar]
                private class FooBarClass
                {
                }

                [Baz]
                private class BazClass
                {
                }

                private class FooChildClass : FooClass
                {
                }
            }
        }
    }
}
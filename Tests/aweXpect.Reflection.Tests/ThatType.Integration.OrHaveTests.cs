using System;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests
{
    public sealed partial class ThatType
    {
        public sealed class Integration
        {
            public sealed class OrHaveTests
            {
                /// <summary>
                /// Test that demonstrates the exact usage pattern requested in the GitHub issue #79
                /// </summary>
                [Fact]
                public async Task ExampleFromIssue_ShouldAllowCombiningMultipleAlternativeAttributes()
                {
                    // This test demonstrates the exact API usage requested in the issue:
                    // "The return value should allow me to directly specify an alternative with `OrHave<TAttribute>()`  
                    // so that I can check for one of multiple options."

                    Type typeWithObsolete = typeof(ClassWithObsolete);
                    Type typeWithDeprecated = typeof(ClassWithDeprecated);
                    Type typeWithNeither = typeof(ClassWithNeither);

                    // Should succeed when type has ObsoleteAttribute
                    async Task Act1()
                        => await That(typeWithObsolete).Has<ObsoleteAttribute>().OrHave<DeprecatedAttribute>();
                    
                    await That(Act1).DoesNotThrow();

                    // Should succeed when type has DeprecatedAttribute  
                    async Task Act2()
                        => await That(typeWithDeprecated).Has<ObsoleteAttribute>().OrHave<DeprecatedAttribute>();
                    
                    await That(Act2).DoesNotThrow();

                    // Should fail when type has neither attribute
                    async Task Act3()
                        => await That(typeWithNeither).Has<ObsoleteAttribute>().OrHave<DeprecatedAttribute>();
                    
                    await That(Act3).Throws<XunitException>();
                }

                [AttributeUsage(AttributeTargets.Class)]
                private class DeprecatedAttribute : Attribute
                {
                }

                [Obsolete]
                private class ClassWithObsolete
                {
                }

                [Deprecated]
                private class ClassWithDeprecated
                {
                }

                private class ClassWithNeither
                {
                }
            }
        }
    }
}
using System.Reflection;
using aweXpect.Reflection.Tests.TestHelpers.Types;

namespace aweXpect.Reflection.Tests;

public sealed class IntegrationTests
{
	[Fact]
	public async Task ExamplesFromIssue_ShouldWork()
	{
		// Test single property expectations as shown in the issue
		PropertyInfo property = typeof(ClassWithSealedMembers).GetProperty(nameof(ClassWithSealedMembers.VirtualProperty))!;
		
		async Task TestPropertyIsNotAbstract()
			=> await That(property).IsNotAbstract();
		
		async Task TestPropertyIsSealed()
			=> await That(property).IsSealed();

		await That(TestPropertyIsNotAbstract).DoesNotThrow();
		await That(TestPropertyIsSealed).DoesNotThrow();
		
		// Test collection expectations using In.Types()
		async Task TestMethodsAreNotAbstract()
			=> await That(In.Type<ClassWithSealedMembers>().Methods()).AreNotAbstract();

		async Task TestPropertiesAreNotSealed() 
			=> await That(In.Type<AbstractClassWithMembers>().Properties()).AreNotSealed();

		await That(TestMethodsAreNotAbstract).DoesNotThrow();
		await That(TestPropertiesAreNotSealed).DoesNotThrow();
	}
}
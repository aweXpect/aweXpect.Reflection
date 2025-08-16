using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatFields
{
	public sealed class Have
	{
		public sealed class AttributeTests
		{
			[Fact]
			public async Task WhenAllFieldsHaveAttribute_ShouldSucceed()
			{
				IEnumerable<FieldInfo> subject = new[]
				{
					typeof(TestClass).GetField("TestField1")!,
					typeof(TestClass).GetField("TestField2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenAllFieldsHaveMatchingAttribute_ShouldSucceed()
			{
				IEnumerable<FieldInfo> subject = new[]
				{
					typeof(TestClass).GetField("TestField1")!,
					typeof(TestClass).GetField("TestField2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value.StartsWith("Field"));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenNotAllFieldsHaveAttribute_ShouldFail()
			{
				IEnumerable<FieldInfo> subject = new[]
				{
					typeof(TestClass).GetField("TestField1")!,
					typeof(TestClass).GetField("NoAttributeField")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have ThatFields.Have.AttributeTests.TestAttribute,
					             but it contained not matching fields [
					               System.String NoAttributeField
					             ]
					             """);
			}

			[Fact]
			public async Task WhenNotAllFieldsHaveMatchingAttribute_ShouldFail()
			{
				IEnumerable<FieldInfo> subject = new[]
				{
					typeof(TestClass).GetField("TestField1")!,
					typeof(TestClass).GetField("TestField2")!
				};

				async Task Act()
					=> await That(subject).Have<TestAttribute>(attr => attr.Value == "WrongValue");

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that subject
					             all have ThatFields.Have.AttributeTests.TestAttribute matching attr => attr.Value == "WrongValue",
					             but it contained not matching fields [
					               System.String TestField1,
					               System.String TestField2
					             ]
					             """);
			}

			[AttributeUsage(AttributeTargets.Field)]
			private class TestAttribute : Attribute
			{
				public string Value { get; set; } = "";
			}

			private class TestClass
			{
				[Test(Value = "Field1Value")]
				public string TestField1 = "";

				[Test(Value = "Field2Value")]
				public string TestField2 = "";

				public string NoAttributeField = "";
			}
		}
	}
}
namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsAClass
	{
		public sealed class Tests
		{
			[Theory]
			[MemberData(nameof(NotClassData))]
			public async Task WhenTypeIsNotAClass_ShouldFail(Type? subject, string name)
			{
				async Task Act()
					=> await That(subject).IsAClass();

				await That(Act).ThrowsException()
					.WithMessage($"""
					             Expected that subject
					             is a class,
					             but it was {name}
					             """);
			}

			[Fact]
			public async Task WhenTypeIsAClass_ShouldSucceed()
			{
				Type subject = typeof(MyClassType);

				async Task Act()
					=> await That(subject).IsAClass();

				await That(Act).DoesNotThrow();
			}

			public static TheoryData<Type?, string> NotClassData() => new()
			{
				{
					null, "<null>"
				},
				{
					typeof(MyStructType), "struct MyStructType"
				},
				{
					typeof(MyInterfaceType), "interface MyInterfaceType"
				},
				{
					typeof(MyEnumType), "enum MyEnumType"
				},
				{
					typeof(MyRecordType), "record MyRecordType"
				},
				{
					typeof(MyRecordStructType), "record struct MyRecordStructType"
				},
			};
		}

		private class MyClassType;

		private struct MyStructType;

		private interface MyInterfaceType;

		private enum MyEnumType;

		private record MyRecordType;

		private record struct MyRecordStructType;
	}
}

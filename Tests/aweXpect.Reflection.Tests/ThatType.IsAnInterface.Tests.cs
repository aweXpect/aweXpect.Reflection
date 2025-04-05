namespace aweXpect.Reflection.Tests;

public sealed partial class ThatType
{
	public sealed class IsAnInterface
	{
		public sealed class Tests
		{
			[Theory]
			[MemberData(nameof(NotInterfaceData))]
			public async Task WhenTypeIsNotAnInterface_ShouldFail(Type? subject, string name)
			{
				async Task Act()
					=> await That(subject).IsAnInterface();

				await That(Act).ThrowsException()
					.WithMessage($"""
					             Expected that subject
					             is an interface,
					             but it was {name}
					             """);
			}

			[Fact]
			public async Task WhenTypeIsAnInterface_ShouldSucceed()
			{
				Type subject = typeof(MyInterfaceType);

				async Task Act()
					=> await That(subject).IsAnInterface();

				await That(Act).DoesNotThrow();
			}

			public static TheoryData<Type?, string> NotInterfaceData() => new()
			{
				{
					null, "<null>"
				},
				{
					typeof(MyStructType), "struct MyStructType"
				},
				{
					typeof(MyClassType), "class MyClassType"
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
	}
}

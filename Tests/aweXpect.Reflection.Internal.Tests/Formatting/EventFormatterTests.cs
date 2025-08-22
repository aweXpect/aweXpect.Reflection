using System.Reflection;
using aweXpect.Reflection.Formatting;
using aweXpect.Reflection.Internal.Tests.TestHelpers;

namespace aweXpect.Reflection.Internal.Tests.Formatting;

public class EventFormatterTests
{
	[Fact]
	public async Task WithCustomEventHandler_ShouldFormatCorrectly()
	{
		EventInfo? eventInfo = typeof(MyTestClass).GetEvent(nameof(MyTestClass.MyEvent));
		EventFormatter formatter = new();

		string result = formatter.GetString(eventInfo);

		await That(result)
			.IsEqualTo("event EventFormatterTests.MyTestClass.MyEventHandler EventFormatterTests.MyTestClass.MyEvent");
	}

	[Fact]
	public async Task WithEventHandler_ShouldFormatCorrectly()
	{
		EventInfo? eventInfo = typeof(MyTestClass).GetEvent(nameof(MyTestClass.PublicEvent));
		EventFormatter formatter = new();

		string result = formatter.GetString(eventInfo);

		await That(result).IsEqualTo("event EventHandler EventFormatterTests.MyTestClass.PublicEvent");
	}

	internal class MyTestClass
	{
		public delegate void MyEventHandler(object? sender, MyEventArgs e);

		public class MyEventArgs : EventArgs
		{
			public int MyValue { get; set; }
		}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
		public event EventHandler PublicEvent;
		public event MyEventHandler MyEvent;
#pragma warning restore CS8618
	}
}

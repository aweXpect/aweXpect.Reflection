namespace aweXpect.Reflection.Tests;

public sealed partial class ThatEvent
{
	public class ClassWithEvents
	{
		public event EventHandler PublicEvent;
		internal event EventHandler InternalEvent;
		protected event EventHandler ProtectedEvent;
		private event EventHandler PrivateEvent;
	}

	public class ClassWithSingleEvent
	{
		public event EventHandler MyEvent;
	}
}

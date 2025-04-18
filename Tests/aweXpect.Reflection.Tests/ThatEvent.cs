﻿using System.Reflection;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatEvent
{
	private static EventInfo? GetEvent(string eventName)
		=> typeof(ClassWithEvents).GetEvent(eventName,
			BindingFlags.DeclaredOnly |
			BindingFlags.NonPublic |
			BindingFlags.Public |
			BindingFlags.Instance);

#pragma warning disable CS8618
#pragma warning disable CS0067
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
#pragma warning restore CS0067
#pragma warning restore CS8618
}

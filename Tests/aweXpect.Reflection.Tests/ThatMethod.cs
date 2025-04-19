﻿namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethod
{
#pragma warning disable CA1822
	public class ClassWithMethods
	{
		public int PublicMethod() => 0;
		internal int InternalMethod() => 0;
		protected int ProtectedMethod() => 0;
		private int PrivateMethod() => 0;
	}

	public class ClassWithSingleMethod
	{
		public int MyMethod() => 0;
	}
#pragma warning restore CA1822
}

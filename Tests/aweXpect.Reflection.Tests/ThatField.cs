namespace aweXpect.Reflection.Tests;

public sealed partial class ThatField
{
	public class ClassWithFields
	{
		internal int InternalField;
		private int PrivateField;
		protected int ProtectedField;
		public int PublicField;
	}

	public class ClassWithSingleField
	{
		public int MyField;
	}
}

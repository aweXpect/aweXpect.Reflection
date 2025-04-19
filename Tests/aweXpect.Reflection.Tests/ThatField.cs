namespace aweXpect.Reflection.Tests;

public sealed partial class ThatField
{
	public class ClassWithFields
	{
		public int PublicField;
		internal int InternalField;
		protected int ProtectedField;
		private int PrivateField;
	}
	public class ClassWithSingleField
	{
		public int MyField;
	}
}

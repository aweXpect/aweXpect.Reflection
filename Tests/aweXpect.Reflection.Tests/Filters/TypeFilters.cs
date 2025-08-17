namespace aweXpect.Reflection.Tests.Filters;

public sealed partial class TypeFilters
{
	private abstract class AbstractTestClass
	{
		public abstract void AbstractMethod();
	}

	private class ConcreteTestClass
	{
		public virtual void VirtualMethod() { }
	}

	private sealed class SealedTestClass
	{
		public void Method() { }
	}
}

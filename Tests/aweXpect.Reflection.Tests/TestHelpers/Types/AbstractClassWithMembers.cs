namespace aweXpect.Reflection.Tests.TestHelpers.Types;

public abstract class AbstractClassWithMembers
{
	public abstract string AbstractProperty { get; set; }
	public virtual string VirtualProperty { get; set; } = "";

	public abstract void AbstractMethod();
	public virtual void VirtualMethod() { }
	public void RegularMethod() { }

#pragma warning disable CS0067 // Event is never used
	public abstract event EventHandler AbstractEvent;
	public virtual event EventHandler? VirtualEvent;
#pragma warning restore CS0067
}

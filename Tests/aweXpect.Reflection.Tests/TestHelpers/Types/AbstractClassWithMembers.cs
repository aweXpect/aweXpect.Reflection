namespace aweXpect.Reflection.Tests.TestHelpers.Types;

public abstract class AbstractClassWithMembers
{
	public abstract string AbstractProperty { get; set; }
	public virtual string VirtualProperty { get; set; } = "";
	
	public abstract void AbstractMethod();
	public virtual void VirtualMethod() { }
	public void RegularMethod() { }
	
	public abstract event EventHandler AbstractEvent;
	public virtual event EventHandler? VirtualEvent;
}
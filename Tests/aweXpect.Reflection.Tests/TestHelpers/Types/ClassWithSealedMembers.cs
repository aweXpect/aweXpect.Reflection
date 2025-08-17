namespace aweXpect.Reflection.Tests.TestHelpers.Types;

public class ClassWithSealedMembers : AbstractClassWithMembers
{
	public override string AbstractProperty { get; set; } = "";
	public sealed override string VirtualProperty { get; set; } = "";
	
	public override void AbstractMethod() { }
	public sealed override void VirtualMethod() { }
	
	public override event EventHandler? AbstractEvent;
	public sealed override event EventHandler? VirtualEvent;
}
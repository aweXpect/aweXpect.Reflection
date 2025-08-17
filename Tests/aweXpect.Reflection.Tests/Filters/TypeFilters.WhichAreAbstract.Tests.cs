using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class TypeFilters
	{
		public sealed class WhichAreAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForAbstractTypes()
				{
					Filtered.Types types = In.AssemblyContaining<TestClasses.AbstractTestClass>()
						.Types().WhichAreAbstract();

					await That(types).AreAbstract();
					await That(types.GetDescription())
						.IsEqualTo("abstract types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonAbstractTypes()
				{
					Filtered.Types types = In.AssemblyContaining<TestClasses.ConcreteTestClass>()
						.Types().WhichAreNotAbstract();

					await That(types).AreNotAbstract();
					await That(types.GetDescription())
						.IsEqualTo("non-abstract types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.TestClasses
{
	public abstract class AbstractTestClass
	{
		public abstract void AbstractMethod();
	}

	public class ConcreteTestClass
	{
		public virtual void VirtualMethod() { }
	}

	public sealed class SealedTestClass
	{
		public void Method() { }
	}

	public interface ITestInterface
	{
		void InterfaceMethod();
	}
}
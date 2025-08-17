using System;
using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class EventFilters
	{
		public sealed class WhichAreAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForAbstractEvents()
				{
					Filtered.Events events = In.AssemblyContaining<EventTestClasses.AbstractEventClass>()
						.Types().Events().WhichAreAbstract();

					await That(events.Count()).IsGreaterThan(0);
					await That(events.GetDescription())
						.IsEqualTo("abstract events in types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotAbstract
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonAbstractEvents()
				{
					Filtered.Events events = In.AssemblyContaining<EventTestClasses.ConcreteEventClass>()
						.Types().Events().WhichAreNotAbstract();

					await That(events.Count()).IsGreaterThan(0);
					await That(events.GetDescription())
						.IsEqualTo("non-abstract events in types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.EventTestClasses
{
	public abstract class AbstractEventClass
	{
		public abstract event Action? AbstractEvent;
		public virtual event Action? VirtualEvent;
	}

	public class ConcreteEventClass
	{
		public virtual event Action? VirtualEvent;
		public event Action? ConcreteEvent;
	}
}
using System;
using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Filters
{
	public sealed partial class EventFilters
	{
		public sealed class WhichAreSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForSealedEvents()
				{
					Filtered.Events events = In.AssemblyContaining<EventTestClasses.SealedEventClass>()
						.Types().Events().WhichAreSealed();

					await That(events.Count()).IsGreaterThan(0);
					await That(events.GetDescription())
						.IsEqualTo("sealed events in types in assembly").AsPrefix();
				}
			}
		}

		public sealed class WhichAreNotSealed
		{
			public sealed class Tests
			{
				[Fact]
				public async Task ShouldAllowFilteringForNonSealedEvents()
				{
					Filtered.Events events = In.AssemblyContaining<EventTestClasses.ConcreteEventClass>()
						.Types().Events().WhichAreNotSealed();

					await That(events.Count()).IsGreaterThan(0);
					await That(events.GetDescription())
						.IsEqualTo("non-sealed events in types in assembly").AsPrefix();
				}
			}
		}
	}
}

namespace aweXpect.Reflection.Tests.Filters.EventTestClasses
{
	public class SealedEventClass : AbstractEventClass
	{
		public sealed override event Action? AbstractEvent;
		public sealed override event Action? VirtualEvent;
	}
}
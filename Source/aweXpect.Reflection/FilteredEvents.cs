using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="EventInfo" />.
/// </summary>
public class FilteredEvents(IEnumerable<EventInfo> source) : IEnumerable<EventInfo>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<EventInfo>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<EventInfo> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all types of the filtered events.
	/// </summary>
	public FilteredTypes Types() => new(this
		.Select(eventInfo => eventInfo.DeclaringType)
		.Where(x => x is not null)
		.Cast<Type>()
		.Distinct());
}

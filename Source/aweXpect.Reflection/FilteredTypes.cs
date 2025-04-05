using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="Type" />.
/// </summary>
public class FilteredTypes(IEnumerable<Type> source) : IEnumerable<Type>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<Type>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<Type> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

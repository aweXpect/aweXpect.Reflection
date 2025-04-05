using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="PropertyInfo" />.
/// </summary>
public class FilteredProperties(IEnumerable<PropertyInfo> source) : IEnumerable<PropertyInfo>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<PropertyInfo>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<PropertyInfo> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all types of the filtered properties.
	/// </summary>
	public FilteredTypes Types() => new(this
		.Select(propertyInfo => propertyInfo.DeclaringType)
		.Where(x => x is not null)
		.Cast<Type>()
		.Distinct());
}

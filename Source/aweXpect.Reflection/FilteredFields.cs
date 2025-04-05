using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="FieldInfo" />.
/// </summary>
public class FilteredFields(IEnumerable<FieldInfo> source) : IEnumerable<FieldInfo>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<FieldInfo>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<FieldInfo> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all types of the filtered fields.
	/// </summary>
	public FilteredTypes Types() => new(this
		.Select(fieldInfo => fieldInfo.DeclaringType)
		.Where(x => x is not null)
		.Cast<Type>()
		.Distinct());
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="MethodInfo" />.
/// </summary>
public class FilteredMethods(IEnumerable<MethodInfo> source) : IEnumerable<MethodInfo>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<MethodInfo>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<MethodInfo> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all types of the filtered methods.
	/// </summary>
	public FilteredTypes Types() => new(this
		.Select(methodInfo => methodInfo.DeclaringType)
		.Where(x => x is not null)
		.Cast<Type>()
		.Distinct());
}

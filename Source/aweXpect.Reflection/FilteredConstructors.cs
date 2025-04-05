using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="ConstructorInfo" />.
/// </summary>
public class FilteredConstructors(IEnumerable<ConstructorInfo> source) : IEnumerable<ConstructorInfo>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<ConstructorInfo>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<ConstructorInfo> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all types of the filtered constructors.
	/// </summary>
	public FilteredTypes Types() => new(this
		.Select(constructorInfo => constructorInfo.DeclaringType)
		.Where(x => x is not null)
		.Cast<Type>()
		.Distinct());
}

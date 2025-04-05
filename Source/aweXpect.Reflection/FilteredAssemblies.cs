using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="Assembly" />.
/// </summary>
public class FilteredAssemblies(IEnumerable<Assembly> source) : IEnumerable<Assembly>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<Assembly>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<Assembly> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all types in the filtered assemblies.
	/// </summary>
	public FilteredTypes Types() => new(this.SelectMany(assembly => assembly.GetTypes()));
}

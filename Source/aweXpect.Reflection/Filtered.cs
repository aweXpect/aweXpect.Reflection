using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace aweXpect.Reflection;

/// <summary>
///     Container for filtered collections.
/// </summary>
public static partial class Filtered;

/// <summary>
///     Base class for filtered collections of <typeparamref name="T" />.
/// </summary>
public abstract class Filtered<T, TFiltered>(IEnumerable<T> source) : IEnumerable<T>
	where TFiltered : Filtered<T, TFiltered>
{
	private readonly List<Filter<T>> _filters = [];

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => source.Where(a => _filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Filters the applicable <typeparamref name="T" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on <typeparamref name="T" />.</param>
	public TFiltered Which(Expression<Func<T, bool>> filter)
	{
		_filters.Add(Filter.FromPredicate(filter));
		return (TFiltered)this;
	}

	/// <summary>
	///     Filters the applicable <typeparamref name="T" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on <typeparamref name="T" />.</param>
	/// <param name="name">The name of the filter.</param>
	public TFiltered Which(Func<T, bool> filter, string name)
	{
		_filters.Add(Filter.FromPredicate(filter, name));
		return (TFiltered)this;
	}
}

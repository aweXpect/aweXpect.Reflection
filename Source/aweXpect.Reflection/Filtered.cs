﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	/// <summary>
	///     The filters on the source.
	/// </summary>
	protected List<Filter<T>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Filters the applicable <typeparamref name="T" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on <typeparamref name="T" />.</param>
	public TFiltered Which(Filter<T> filter)
	{
		Filters.Add(filter);
		return (TFiltered)this;
	}
}

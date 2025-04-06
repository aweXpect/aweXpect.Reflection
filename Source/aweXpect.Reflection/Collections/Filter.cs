using System;

namespace aweXpect.Reflection.Collections;

/// <summary>
///     Filters specify which entities must satisfy the requirements.
/// </summary>
public static class Filter
{
	/// <summary>
	///     Creates a new <see cref="Filter{TEntity}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TEntity> Prefix<TEntity>(Func<TEntity, bool> predicate, string prefix)
		=> new GenericPrefixFilter<TEntity>(predicate, prefix);

	/// <summary>
	///     Creates a new <see cref="Filter{TEntity}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TEntity> Suffix<TEntity>(Func<TEntity, bool> predicate, string suffix)
		=> new GenericSuffixFilter<TEntity>(predicate, suffix);

	private sealed class GenericPrefixFilter<TEntity> : Filter<TEntity>
	{
		private readonly Func<TEntity, bool> _filter;
		private readonly string _prefix;

		public GenericPrefixFilter(Func<TEntity, bool> filter, string prefix)
		{
			_filter = filter;
			_prefix = prefix;
		}

		/// <inheritdoc cref="Filter{TEntity}.Applies(TEntity)" />
		public override bool Applies(TEntity type)
			=> _filter(type);

		/// <inheritdoc cref="Filter{TEntity}.Describe(string)" />
		public override string Describe(string text)
			=> _prefix + text;

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> _prefix;
	}

	private sealed class GenericSuffixFilter<TEntity> : Filter<TEntity>
	{
		private readonly Func<TEntity, bool> _filter;
		private readonly string _suffix;

		public GenericSuffixFilter(Func<TEntity, bool> filter, string suffix)
		{
			_filter = filter;
			_suffix = suffix;
		}

		/// <inheritdoc cref="Filter{TEntity}.Applies(TEntity)" />
		public override bool Applies(TEntity type)
			=> _filter(type);

		/// <inheritdoc cref="Filter{TEntity}.Describe(string)" />
		public override string Describe(string text)
			=> text + _suffix;

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> _suffix;
	}
}

/// <summary>
///     Filter for <typeparamref name="TEntity" />.
/// </summary>
public abstract class Filter<TEntity>
{
	/// <summary>
	///     Checks if the filter applies to the given <typeparamref name="TEntity" />.
	/// </summary>
	public abstract bool Applies(TEntity type);

	/// <summary>
	///     Describes the filter around the given <paramref name="text" />.
	/// </summary>
	public abstract string Describe(string text);
}

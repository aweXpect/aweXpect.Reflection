using System;
using System.Linq.Expressions;

namespace aweXpect.Reflection;

/// <summary>
///     Filters specify which entities must satisfy the requirements.
/// </summary>
public static class Filter
{
	/// <summary>
	///     Creates a new <see cref="Filter{TEntity}" /> from the given <paramref name="predicate" /> and
	///     uses the expression as name.
	/// </summary>
	public static Filter<TEntity> FromPredicate<TEntity>(Expression<Func<TEntity, bool>> predicate)
	{
		Func<TEntity, bool> compiledPredicate = predicate.Compile();
		return new GenericFilter<TEntity>(compiledPredicate, predicate.ToString());
	}

	/// <summary>
	///     Creates a new <see cref="Filter{TEntity}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TEntity> FromPredicate<TEntity>(Func<TEntity, bool> predicate, string name)
		=> new GenericFilter<TEntity>(predicate, name);

	private sealed class GenericFilter<TEntity> : Filter<TEntity>
	{
		private readonly Func<TEntity, bool> _filter;
		private readonly string _name;

		public GenericFilter(Func<TEntity, bool> filter, string name)
		{
			_filter = filter;
			_name = name;
		}

		/// <inheritdoc cref="Filter{TEntity}.Applies(TEntity)" />
		public override bool Applies(TEntity type)
			=> _filter(type);

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> _name;
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
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace aweXpect.Reflection.Options;

/// <summary>
///     Options for adding additional predicates to filter the generic argument list.
/// </summary>
public class GenericArgumentFilterOptions(Func<Type, bool> predicate, Func<string> description)
{
	private readonly List<Func<string>> _descriptions = [description,];
	private readonly List<Func<Type, bool>> _predicates = [predicate,];
	private readonly List<Func<int, bool>> _countPredicates = [];
	private readonly List<Func<string>> _countDescriptions = [];

	/// <summary>
	///     Adds an additional <paramref name="predicate" /> with the <paramref name="description" />.
	/// </summary>
	public void AddPredicate(Func<Type, bool> predicate, Func<string> description)
	{
		_predicates.Add(predicate);
		_descriptions.Add(description);
	}

	/// <summary>
	///     Adds a count predicate with the <paramref name="description" />.
	/// </summary>
	public void AddCountPredicate(Func<int, bool> predicate, Func<string> description)
	{
		_countPredicates.Add(predicate);
		_countDescriptions.Add(description);
	}

	/// <summary>
	///     Verifies that the <paramref name="genericArgument" /> matches all predicates.
	/// </summary>
	public bool Matches(Type genericArgument)
	{
		if (_predicates.Count == 0)
		{
			return true;
		}

		return _predicates.All(predicate => predicate(genericArgument));
	}

	/// <summary>
	///     Verifies that the <paramref name="count" /> matches all count predicates.
	/// </summary>
	public bool MatchesCount(int count)
	{
		if (_countPredicates.Count == 0)
		{
			return true;
		}

		return _countPredicates.All(predicate => predicate(count));
	}

	/// <summary>
	///     Returns the combination of all descriptions joined by <c>" and "</c>.
	/// </summary>
	public string GetDescription()
	{
		var allDescriptions = _descriptions.Concat(_countDescriptions);
		return string.Join(" and ", allDescriptions.Select(@delegate => @delegate()));
	}
}
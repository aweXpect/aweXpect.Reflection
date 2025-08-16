using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection.Helpers;

internal class ParameterFilterOptions(Func<ParameterInfo, bool> predicate, Func<string> description)
{
	private readonly List<Func<string>> _descriptions = [description,];
	private readonly List<Func<ParameterInfo, bool>> _predicates = [predicate,];

	public void AddPredicate(Func<ParameterInfo, bool> predicate, Func<string> description)
	{
		_predicates.Add(predicate);
		_descriptions.Add(description);
	}

	public bool Matches(ParameterInfo parameter)
	{
		if (_predicates.Count == 0)
		{
			return true;
		}

		return _predicates.All(predicate => predicate(parameter));
	}

	public string GetDescription()
		=> string.Join(" and ", _descriptions.Select(@delegate => @delegate()));
}

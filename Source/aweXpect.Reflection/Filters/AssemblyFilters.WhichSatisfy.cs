﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class AssemblyFilters
{
	/// <summary>
	///     Filters the assemblies according to the <paramref name="predicate" />.
	/// </summary>
	public static Filtered.Assemblies WhichSatisfy(this Filtered.Assemblies typeAssemblies,
		Func<Assembly, bool> predicate,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
		=> typeAssemblies.Which(Filter.Suffix(predicate, $" matching {doNotPopulateThisValue}"));
}

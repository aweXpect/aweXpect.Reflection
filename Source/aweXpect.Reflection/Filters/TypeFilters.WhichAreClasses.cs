using System;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class TypeFilters
{
	/// <summary>
	///     Filters for types that are classes.
	/// </summary>
	public static Filtered.Types WhichAreClasses(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => type.IsReallyClass(),
			"classes "));

	/// <summary>
	///     Filters for types that are not classes.
	/// </summary>
	public static Filtered.Types WhichAreNotClasses(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => !type.IsReallyClass(),
			"non-class "));
}
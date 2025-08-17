using System;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class TypeFilters
{
	/// <summary>
	///     Filters for types that are interfaces.
	/// </summary>
	public static Filtered.Types WhichAreInterfaces(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => type?.IsInterface == true,
			"interface "));

	/// <summary>
	///     Filters for types that are not interfaces.
	/// </summary>
	public static Filtered.Types WhichAreNotInterfaces(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => type?.IsInterface != true,
			"non-interface "));
}
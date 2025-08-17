using System;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class TypeFilters
{
	/// <summary>
	///     Filters for types that are enums.
	/// </summary>
	public static Filtered.Types WhichAreEnums(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => type?.IsEnum == true,
			"enum "));

	/// <summary>
	///     Filters for types that are not enums.
	/// </summary>
	public static Filtered.Types WhichAreNotEnums(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => type?.IsEnum != true,
			"non-enum "));
}
using System;

namespace aweXpect.Reflection;

public static partial class FilteredExtensions
{
	/// <summary>
	///     Filter for abstract types.
	/// </summary>
	public static Filtered.Types WhichAreAbstract(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(type => type.IsAbstract, "abstract "));
}

using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class MethodFilters
{
	/// <summary>
	///     Filters for methods that are sealed.
	/// </summary>
	public static Filtered.Methods WhichAreSealed(this Filtered.Methods @this)
		=> @this.Which(Filter.Prefix<MethodInfo>(
			method => method.IsReallySealed(),
			"sealed "));

	/// <summary>
	///     Filters for methods that are not sealed.
	/// </summary>
	public static Filtered.Methods WhichAreNotSealed(this Filtered.Methods @this)
		=> @this.Which(Filter.Prefix<MethodInfo>(
			method => !method.IsReallySealed(),
			"non-sealed "));
}
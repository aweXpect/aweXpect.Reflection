using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class MethodFilters
{
	/// <summary>
	///     Filters for methods that are abstract.
	/// </summary>
	public static Filtered.Methods WhichAreAbstract(this Filtered.Methods @this)
		=> @this.Which(Filter.Prefix<MethodInfo>(
			method => method.IsReallyAbstract(),
			"abstract "));

	/// <summary>
	///     Filters for methods that are not abstract.
	/// </summary>
	public static Filtered.Methods WhichAreNotAbstract(this Filtered.Methods @this)
		=> @this.Which(Filter.Prefix<MethodInfo>(
			method => !method.IsReallyAbstract(),
			"non-abstract "));
}
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class PropertyFilters
{
	/// <summary>
	///     Filters for properties that are static.
	/// </summary>
	public static Filtered.Properties WhichAreStatic(this Filtered.Properties @this)
		=> @this.Which(Filter.Prefix<PropertyInfo>(
			property => (property.GetMethod?.IsStatic ?? false) || (property.SetMethod?.IsStatic ?? false),
			"static "));

	/// <summary>
	///     Filters for properties that are not static.
	/// </summary>
	public static Filtered.Properties WhichAreNotStatic(this Filtered.Properties @this)
		=> @this.Which(Filter.Prefix<PropertyInfo>(
			property => !(property.GetMethod?.IsStatic ?? false) && !(property.SetMethod?.IsStatic ?? false),
			"non-static "));
}
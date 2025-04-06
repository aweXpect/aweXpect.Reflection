using System;

namespace aweXpect.Reflection.Collections;

public static partial class FilteredExtensions
{
	/// <summary>
	///     Get all abstract types in the filtered assemblies.
	/// </summary>
	public static Filtered.Types AbstractTypes(this Filtered.Assemblies assemblies)
		=> assemblies.Types().Which(Filter.Prefix<Type>(type => type.IsAbstract, "abstract "));
}

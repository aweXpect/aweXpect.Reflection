using System;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class TypeFilters
{
	/// <summary>
	///     Filters for types that have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Types WhichAre(this Filtered.Types @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<Type>(
			type => type.HasAccessModifier(accessModifier),
			accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for types that do not have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Types WhichAreNot(this Filtered.Types @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<Type>(
			type => !type.HasAccessModifier(accessModifier),
			"non-" + accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for types that are generic.
	/// </summary>
	public static Filtered.Types WhichAreGeneric(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => type.IsGenericType,
			"generic "));

	/// <summary>
	///     Filters for types that are not generic.
	/// </summary>
	public static Filtered.Types WhichAreNotGeneric(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => !type.IsGenericType,
			"non-generic "));
}

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
	///     Filters for types that are public.
	/// </summary>
	public static Filtered.Types WhichArePublic(this Filtered.Types @this)
		=> @this.WhichAre(AccessModifiers.Public);

	/// <summary>
	///     Filters for types that are private.
	/// </summary>
	public static Filtered.Types WhichArePrivate(this Filtered.Types @this)
		=> @this.WhichAre(AccessModifiers.Private);

	/// <summary>
	///     Filters for types that are protected.
	/// </summary>
	public static Filtered.Types WhichAreProtected(this Filtered.Types @this)
		=> @this.WhichAre(AccessModifiers.Protected);

	/// <summary>
	///     Filters for types that are internal.
	/// </summary>
	public static Filtered.Types WhichAreInternal(this Filtered.Types @this)
		=> @this.WhichAre(AccessModifiers.Internal);
}

using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class PropertyFilters
{
	/// <summary>
	///     Filters for properties that have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Properties WhichAre(this Filtered.Properties @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<PropertyInfo>(
			type => type.HasAccessModifier(accessModifier),
			accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for properties that do not have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Properties WhichAreNot(this Filtered.Properties @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<PropertyInfo>(
			type => !type.HasAccessModifier(accessModifier),
			"non-" + accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for properties that are public.
	/// </summary>
	public static Filtered.Properties WhichArePublic(this Filtered.Properties @this)
		=> @this.WhichAre(AccessModifiers.Public);

	/// <summary>
	///     Filters for properties that are private.
	/// </summary>
	public static Filtered.Properties WhichArePrivate(this Filtered.Properties @this)
		=> @this.WhichAre(AccessModifiers.Private);

	/// <summary>
	///     Filters for properties that are protected.
	/// </summary>
	public static Filtered.Properties WhichAreProtected(this Filtered.Properties @this)
		=> @this.WhichAre(AccessModifiers.Protected);

	/// <summary>
	///     Filters for properties that are internal.
	/// </summary>
	public static Filtered.Properties WhichAreInternal(this Filtered.Properties @this)
		=> @this.WhichAre(AccessModifiers.Internal);
}

using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class FieldFilters
{
	/// <summary>
	///     Filters for fields that have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Fields WhichAre(this Filtered.Fields @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<FieldInfo>(
			type => type.HasAccessModifier(accessModifier),
			accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for fields that do not have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Fields WhichAreNot(this Filtered.Fields @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<FieldInfo>(
			type => !type.HasAccessModifier(accessModifier),
			"non-" + accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for fields that are public.
	/// </summary>
	public static Filtered.Fields WhichArePublic(this Filtered.Fields @this)
		=> @this.WhichAre(AccessModifiers.Public);

	/// <summary>
	///     Filters for fields that are private.
	/// </summary>
	public static Filtered.Fields WhichArePrivate(this Filtered.Fields @this)
		=> @this.WhichAre(AccessModifiers.Private);

	/// <summary>
	///     Filters for fields that are protected.
	/// </summary>
	public static Filtered.Fields WhichAreProtected(this Filtered.Fields @this)
		=> @this.WhichAre(AccessModifiers.Protected);

	/// <summary>
	///     Filters for fields that are internal.
	/// </summary>
	public static Filtered.Fields WhichAreInternal(this Filtered.Fields @this)
		=> @this.WhichAre(AccessModifiers.Internal);

	/// <summary>
	///     Filters for fields that are not public.
	/// </summary>
	public static Filtered.Fields WhichAreNotPublic(this Filtered.Fields @this)
		=> @this.WhichAreNot(AccessModifiers.Public);

	/// <summary>
	///     Filters for fields that are not private.
	/// </summary>
	public static Filtered.Fields WhichAreNotPrivate(this Filtered.Fields @this)
		=> @this.WhichAreNot(AccessModifiers.Private);

	/// <summary>
	///     Filters for fields that are not protected.
	/// </summary>
	public static Filtered.Fields WhichAreNotProtected(this Filtered.Fields @this)
		=> @this.WhichAreNot(AccessModifiers.Protected);

	/// <summary>
	///     Filters for fields that are not internal.
	/// </summary>
	public static Filtered.Fields WhichAreNotInternal(this Filtered.Fields @this)
		=> @this.WhichAreNot(AccessModifiers.Internal);
}

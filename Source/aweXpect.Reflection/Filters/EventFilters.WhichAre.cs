using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class EventFilters
{
	/// <summary>
	///     Filters for events that have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Events WhichAre(this Filtered.Events @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<EventInfo>(
			type => type.HasAccessModifier(accessModifier),
			accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for events that do not have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Events WhichAreNot(this Filtered.Events @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<EventInfo>(
			type => !type.HasAccessModifier(accessModifier),
			"non-" + accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for events that are public.
	/// </summary>
	public static Filtered.Events WhichArePublic(this Filtered.Events @this)
		=> @this.WhichAre(AccessModifiers.Public);

	/// <summary>
	///     Filters for events that are private.
	/// </summary>
	public static Filtered.Events WhichArePrivate(this Filtered.Events @this)
		=> @this.WhichAre(AccessModifiers.Private);

	/// <summary>
	///     Filters for events that are protected.
	/// </summary>
	public static Filtered.Events WhichAreProtected(this Filtered.Events @this)
		=> @this.WhichAre(AccessModifiers.Protected);

	/// <summary>
	///     Filters for events that are internal.
	/// </summary>
	public static Filtered.Events WhichAreInternal(this Filtered.Events @this)
		=> @this.WhichAre(AccessModifiers.Internal);

	/// <summary>
	///     Filters for events that are not public.
	/// </summary>
	public static Filtered.Events WhichAreNotPublic(this Filtered.Events @this)
		=> @this.WhichAreNot(AccessModifiers.Public);

	/// <summary>
	///     Filters for events that are not private.
	/// </summary>
	public static Filtered.Events WhichAreNotPrivate(this Filtered.Events @this)
		=> @this.WhichAreNot(AccessModifiers.Private);

	/// <summary>
	///     Filters for events that are not protected.
	/// </summary>
	public static Filtered.Events WhichAreNotProtected(this Filtered.Events @this)
		=> @this.WhichAreNot(AccessModifiers.Protected);

	/// <summary>
	///     Filters for events that are not internal.
	/// </summary>
	public static Filtered.Events WhichAreNotInternal(this Filtered.Events @this)
		=> @this.WhichAreNot(AccessModifiers.Internal);
}

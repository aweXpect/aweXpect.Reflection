using System.Reflection;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class ConstructorFilters
{
	/// <summary>
	///     Filters for constructors that have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Constructors WhichAre(this Filtered.Constructors @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<ConstructorInfo>(
			type => type.HasAccessModifier(accessModifier),
			accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for constructors that do not have the given <paramref name="accessModifier" />.
	/// </summary>
	public static Filtered.Constructors WhichAreNot(this Filtered.Constructors @this,
		AccessModifiers accessModifier)
		=> @this.Which(Filter.Prefix<ConstructorInfo>(
			type => !type.HasAccessModifier(accessModifier),
			"non-" + accessModifier.GetString(" ")));

	/// <summary>
	///     Filters for constructors that are public.
	/// </summary>
	public static Filtered.Constructors WhichArePublic(this Filtered.Constructors @this)
		=> @this.WhichAre(AccessModifiers.Public);

	/// <summary>
	///     Filters for constructors that are private.
	/// </summary>
	public static Filtered.Constructors WhichArePrivate(this Filtered.Constructors @this)
		=> @this.WhichAre(AccessModifiers.Private);

	/// <summary>
	///     Filters for constructors that are protected.
	/// </summary>
	public static Filtered.Constructors WhichAreProtected(this Filtered.Constructors @this)
		=> @this.WhichAre(AccessModifiers.Protected);

	/// <summary>
	///     Filters for constructors that are internal.
	/// </summary>
	public static Filtered.Constructors WhichAreInternal(this Filtered.Constructors @this)
		=> @this.WhichAre(AccessModifiers.Internal);
}

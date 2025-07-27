namespace aweXpect.Reflection.Collections;

/// <summary>
///     An interface to allow filtering for types
/// </summary>
public interface ITypeAssemblies : ILimitedTypeAssemblies<ITypeAssemblies>
{
	/// <summary>
	///     Filters only for abstract types.
	/// </summary>
	ILimitedTypeAssemblies<ILimitedTypeAssemblies> Abstract { get; }

	/// <summary>
	///     Filters only for sealed types.
	/// </summary>
	ILimitedTypeAssemblies<ILimitedTypeAssemblies> Sealed { get; }

	/// <summary>
	///     Filters only for static types.
	/// </summary>
	ILimitedTypeAssemblies<ILimitedTypeAssemblies> Static { get; }

	/// <summary>
	///     Get all interfaces in the filtered assemblies.
	/// </summary>
	Filtered.Types Interfaces(AccessModifiers accessModifier = AccessModifiers.Any);

	/// <summary>
	///     Get all enums in the filtered assemblies.
	/// </summary>
	Filtered.Types Enums(AccessModifiers accessModifier = AccessModifiers.Any);
}

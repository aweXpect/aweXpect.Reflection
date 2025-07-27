namespace aweXpect.Reflection.Collections;

/// <summary>
///     An interface to allow filtering for types
/// </summary>
public interface ILimitedTypeAssemblies
{
	/// <summary>
	///     Get all types in the filtered assemblies.
	/// </summary>
	Filtered.Types Types(AccessModifiers accessModifier = AccessModifiers.Any);

	/// <summary>
	///     Get all classes in the filtered assemblies.
	/// </summary>
	Filtered.Types Classes(AccessModifiers accessModifier = AccessModifiers.Any);
}

/// <summary>
///     An interface to allow filtering for types
/// </summary>
public interface ILimitedTypeAssemblies<out TLimitedTypeAssemblies> : ILimitedTypeAssemblies
	where TLimitedTypeAssemblies : ILimitedTypeAssemblies
{
	/// <summary>
	///     Filters only for generic types.
	/// </summary>
	TLimitedTypeAssemblies Generic { get; }

	/// <summary>
	///     Filters only for nested types.
	/// </summary>
	TLimitedTypeAssemblies Nested { get; }
}

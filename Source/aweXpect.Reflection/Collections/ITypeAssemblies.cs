﻿namespace aweXpect.Reflection.Collections;

/// <summary>
///     An interface to allow filtering for types in assemblies.
/// </summary>
/// <remarks>
///     In addition to the properties and methods in <see cref="ILimitedTypeAssemblies{ITypeAssemblies}" /> it also
///     supports adding a filter for abstract, sealed or static types as well as accessing interfaces or enums.
/// </remarks>
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

	/// <summary>
	///     An interface to allow filtering for types in assemblies.
	/// </summary>
	/// <remarks>
	///     In addition to the properties and methods in <see cref="ITypeAssemblies" /> it also
	///     allows specifying a private protected access modifier.
	/// </remarks>
	public interface IPrivate : ITypeAssemblies
	{
		/// <summary>
		///     Filters only for private protected types.
		/// </summary>
		ITypeAssemblies Protected { get; }
	}

	/// <summary>
	///     An interface to allow filtering for types in assemblies.
	/// </summary>
	/// <remarks>
	///     In addition to the properties and methods in <see cref="ITypeAssemblies" /> it also
	///     allows specifying a protected internal access modifier.
	/// </remarks>
	public interface IProtected : ITypeAssemblies
	{
		/// <summary>
		///     Filters only for protected internal types.
		/// </summary>
		ITypeAssemblies Internal { get; }
	}
}

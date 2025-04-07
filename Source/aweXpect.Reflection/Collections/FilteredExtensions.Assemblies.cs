﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using aweXpect.Reflection.Extensions;

namespace aweXpect.Reflection.Collections;

public static partial class FilteredExtensions
{
	/// <summary>
	///     Filters the assemblies according to the <paramref name="predicate"/>.
	/// </summary>
	public static Filtered.Assemblies WhichSatisfy(this Filtered.Assemblies assemblies,
		Func<Assembly, bool> predicate,
		[CallerArgumentExpression("predicate")] string doNotPopulateThisValue = "")
		=> assemblies.Which(Filter.Suffix(predicate, $" matching {doNotPopulateThisValue}"));
	
	/// <summary>
	///     Get all abstract types in the filtered assemblies with the given <paramref name="accessModifier"/>.
	/// </summary>
	public static Filtered.Types AbstractTypes(this Filtered.Assemblies assemblies,
		AccessModifiers accessModifier = AccessModifiers.Any)
	{
		Func<Type, bool> predicate = type => accessModifier == AccessModifiers.Any
			? type.IsAbstract
			: type.IsAbstract && type.HasAccessModifier(accessModifier);
		return assemblies.Types().Which(Filter.Prefix(predicate, $"{accessModifier.GetString()}abstract "));
	}
	
	/// <summary>
	///     Get all nested types in the filtered assemblies with the given <paramref name="accessModifier"/>.
	/// </summary>
	public static Filtered.Types NestedTypes(this Filtered.Assemblies assemblies,
		AccessModifiers accessModifier = AccessModifiers.Any)
	{
		Func<Type, bool> predicate = type => accessModifier == AccessModifiers.Any
			? type.IsNested
			: type.IsNested && type.HasAccessModifier(accessModifier);
		return assemblies.Types().Which(Filter.Prefix(predicate, $"{accessModifier.GetString()}nested "));
	}

	/// <summary>
	///     Get all sealed types in the filtered assemblies with the given <paramref name="accessModifier"/>.
	/// </summary>
	public static Filtered.Types SealedTypes(this Filtered.Assemblies assemblies,
		AccessModifiers accessModifier = AccessModifiers.Any)
	{
		Func<Type, bool> predicate = type => accessModifier == AccessModifiers.Any
			? type.IsSealed
			: type.IsSealed && type.HasAccessModifier(accessModifier);
		return assemblies.Types().Which(Filter.Prefix(predicate, $"{accessModifier.GetString()}sealed "));
	}
	
	/// <summary>
	///     Get all static types in the filtered assemblies with the given <paramref name="accessModifier"/>.
	/// </summary>
	public static Filtered.Types StaticTypes(this Filtered.Assemblies assemblies,
		AccessModifiers accessModifier = AccessModifiers.Any)
	{
		Func<Type, bool> predicate = type => accessModifier == AccessModifiers.Any
			? type.IsStatic()
			: type.IsStatic() && type.HasAccessModifier(accessModifier);
		return assemblies.Types().Which(Filter.Prefix(predicate, $"{accessModifier.GetString()}static "));
	}
}

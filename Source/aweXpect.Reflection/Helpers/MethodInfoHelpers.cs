﻿using System;
using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Helpers;

/// <summary>
///     Extension methods for <see cref="MethodInfo" />.
/// </summary>
internal static class MethodInfoHelpers
{
	/// <summary>
	///     Checks if the <paramref name="methodInfo" /> has the specified <paramref name="accessModifiers" />.
	/// </summary>
	/// <param name="methodInfo">The <see cref="MethodInfo" /> which is checked to have the attribute.</param>
	/// <param name="accessModifiers">
	///     The <see cref="AccessModifiers" />.
	///     <para />
	///     Supports specifying multiple <see cref="AccessModifiers" />.
	/// </param>
	public static bool HasAccessModifier(
		this MethodInfo? methodInfo,
		AccessModifiers accessModifiers)
	{
		if (methodInfo == null)
		{
			return false;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Internal) &&
		    methodInfo.IsAssembly)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Protected) &&
		    methodInfo.IsFamily)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Private) &&
		    methodInfo.IsPrivate)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Public) &&
		    methodInfo.IsPublic)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	///     Checks if the <paramref name="methodInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="methodInfo">The <see cref="MethodInfo" /> which is checked to have the attribute.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static bool HasAttribute<TAttribute>(
		this MethodInfo methodInfo,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		object? attribute = Attribute.GetCustomAttributes(methodInfo, typeof(TAttribute), inherit)
			.FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}
}

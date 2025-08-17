﻿using System;
using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Helpers;

/// <summary>
///     Extension fields for <see cref="FieldInfo" />.
/// </summary>
internal static class FieldInfoHelpers
{
	/// <summary>
	///     Checks if the <paramref name="fieldInfo" /> has the specified <paramref name="accessModifiers" />.
	/// </summary>
	/// <param name="fieldInfo">The <see cref="FieldInfo" /> which is checked to have the attribute.</param>
	/// <param name="accessModifiers">
	///     The <see cref="AccessModifiers" />.
	///     <para />
	///     Supports specifying multiple <see cref="AccessModifiers" />.
	/// </param>
	public static bool HasAccessModifier(
		this FieldInfo? fieldInfo,
		AccessModifiers accessModifiers)
	{
		if (fieldInfo == null)
		{
			return false;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Internal) &&
		    fieldInfo.IsAssembly)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Protected) &&
		    fieldInfo.IsFamily)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Private) &&
		    fieldInfo.IsPrivate)
		{
			return true;
		}

		if (accessModifiers.HasFlag(AccessModifiers.Public) &&
		    fieldInfo.IsPublic)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	///     Checks if the <paramref name="fieldInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="fieldInfo">The <see cref="FieldInfo" /> which is checked to have the attribute.</param>
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
		this FieldInfo fieldInfo,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		object? attribute = fieldInfo.GetCustomAttributes(typeof(TAttribute), inherit)
			.FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}

	/// <summary>
	///     Checks if the <paramref name="fieldInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <param name="fieldInfo">The <see cref="FieldInfo" /> which is checked to have the attribute.</param>
	/// <param name="attributeType">The type of the attribute to check for.</param>
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
	public static bool HasAttribute(
		this FieldInfo? fieldInfo,
		Type attributeType,
		Func<Attribute, bool>? predicate = null,
		bool inherit = true)
	{
		object? attribute = fieldInfo?.GetCustomAttributes(attributeType, inherit)
			.FirstOrDefault();
		if (attribute is Attribute attributeValue)
		{
			return predicate?.Invoke(attributeValue) ?? true;
		}

		return false;
	}

	/// <summary>
	///     Checks if the <paramref name="fieldInfo" /> is static.
	/// </summary>
	/// <param name="fieldInfo">The <see cref="FieldInfo" /> to check.</param>
	public static bool IsReallyStatic(this FieldInfo? fieldInfo)
		=> fieldInfo?.IsStatic == true;
}

﻿using System;
using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Helpers;

/// <summary>
///     Extension properties for <see cref="PropertyInfo" />.
/// </summary>
internal static class PropertyInfoHelpers
{
	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> has the specified <paramref name="accessModifiers" />.
	/// </summary>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> which is checked to have the attribute.</param>
	/// <param name="accessModifiers">
	///     The <see cref="AccessModifiers" />.
	///     <para />
	///     Supports specifying multiple <see cref="AccessModifiers" />.
	/// </param>
	public static bool HasAccessModifier(
		this PropertyInfo? propertyInfo,
		AccessModifiers accessModifiers)
	{
		if (propertyInfo == null)
		{
			return false;
		}

		return propertyInfo.GetMethod.HasAccessModifier(accessModifiers) &&
		       propertyInfo.SetMethod.HasAccessModifier(accessModifiers);
	}

	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <typeparam name="TAttribute">The type of the <see cref="Attribute" />.</typeparam>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> which is checked to have the attribute.</param>
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
		this PropertyInfo propertyInfo,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		object? attribute = Attribute.GetCustomAttributes(propertyInfo, typeof(TAttribute), inherit)
			.FirstOrDefault();
		if (attribute is TAttribute castedAttribute)
		{
			return predicate?.Invoke(castedAttribute) ?? true;
		}

		return false;
	}

	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> has an attribute which satisfies the <paramref name="predicate" />.
	/// </summary>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> which is checked to have the attribute.</param>
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
		this PropertyInfo? propertyInfo,
		Type attributeType,
		Func<Attribute, bool>? predicate = null,
		bool inherit = true)
	{
		object? attribute = propertyInfo?.GetCustomAttributes(attributeType, inherit)
			.FirstOrDefault();
		if (attribute is Attribute attributeValue)
		{
			return predicate?.Invoke(attributeValue) ?? true;
		}

		return false;
	}

	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> is abstract (based on its accessor methods).
	/// </summary>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> to check.</param>
	/// <returns><see langword="true" /> if the property is abstract; otherwise, <see langword="false" />.</returns>
	public static bool IsReallyAbstract(this PropertyInfo? propertyInfo)
		=> propertyInfo?.GetMethod?.IsReallyAbstract() == true ||
		   propertyInfo?.SetMethod?.IsReallyAbstract() == true;

	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> is sealed (based on its accessor methods).
	/// </summary>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> to check.</param>
	/// <returns><see langword="true" /> if the property is sealed; otherwise, <see langword="false" />.</returns>
	public static bool IsReallySealed(this PropertyInfo? propertyInfo)
		=> propertyInfo?.GetMethod?.IsReallySealed() == true ||
		   propertyInfo?.SetMethod?.IsReallySealed() == true;

	/// <summary>
	///     Checks if the <paramref name="propertyInfo" /> is static.
	/// </summary>
	/// <param name="propertyInfo">The <see cref="PropertyInfo" /> which is checked to be static.</param>
	public static bool IsReallyStatic(
		this PropertyInfo? propertyInfo)
		=> (propertyInfo?.GetMethod?.IsStatic ?? false) || (propertyInfo?.SetMethod?.IsStatic ?? false);
}

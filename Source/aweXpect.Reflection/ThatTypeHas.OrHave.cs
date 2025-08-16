using System;
using System.Runtime.CompilerServices;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Reflection.Helpers;
using aweXpect.Results;

namespace aweXpect.Reflection;

public static partial class ThatType
{
	/// <summary>
	///     Allows an alternative attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <remarks>
	///     The optional parameter <paramref name="inherit" /> (default value <see langword="true" /> specifies, if
	///     the attribute can be inherited from a base type.
	/// </remarks>
	public static AndOrResult<Type?, IThat<Type?>> OrHave<TAttribute>(
		this AndOrResult<Type?, IThat<Type?>> result,
		bool inherit = true)
		where TAttribute : Attribute
	{
		// Use the Or property to create OR logic instead of AND logic
		return result.Or.Has<TAttribute>(inherit);
	}

	/// <summary>
	///     Allows an alternative attribute of type <typeparamref name="TAttribute" /> that
	///     matches the <paramref name="predicate" />.
	/// </summary>
	/// <remarks>
	///     The optional parameter <paramref name="inherit" /> (default value <see langword="true" /> specifies, if
	///     the attribute can be inherited from a base type.
	/// </remarks>
	public static AndOrResult<Type?, IThat<Type?>> OrHave<TAttribute>(
		this AndOrResult<Type?, IThat<Type?>> result,
		Func<TAttribute, bool>? predicate,
		bool inherit = true,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
		where TAttribute : Attribute
	{
		// Use the Or property to create OR logic instead of AND logic
		return result.Or.Has<TAttribute>(predicate, inherit, doNotPopulateThisValue);
	}
}
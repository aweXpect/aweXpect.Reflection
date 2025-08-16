using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using aweXpect.Core;
using aweXpect.Results;

namespace aweXpect.Reflection;

/// <summary>
///     Result of a Has&lt;TAttribute&gt; operation that allows chaining with OrHave methods.
/// </summary>
public sealed class HasAttributeResult
{
	private readonly AndOrResult<Type?, IThat<Type?>> _result;

	internal HasAttributeResult(AndOrResult<Type?, IThat<Type?>> result)
	{
		_result = result;
	}

	/// <summary>
	///     Allows an alternative attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <remarks>
	///     The optional parameter <paramref name="inherit" /> (default value <see langword="true" />) specifies, if
	///     the attribute can be inherited from a base type.
	/// </remarks>
	public HasAttributeResult OrHave<TAttribute>(bool inherit = true)
		where TAttribute : Attribute
	{
		// Use the Or property to create OR logic instead of AND logic
		return new HasAttributeResult(_result.Or.Has<TAttribute>(inherit));
	}

	/// <summary>
	///     Allows an alternative attribute of type <typeparamref name="TAttribute" /> that
	///     matches the <paramref name="predicate" />.
	/// </summary>
	/// <remarks>
	///     The optional parameter <paramref name="inherit" /> (default value <see langword="true" />) specifies, if
	///     the attribute can be inherited from a base type.
	/// </remarks>
	public HasAttributeResult OrHave<TAttribute>(
		Func<TAttribute, bool>? predicate,
		bool inherit = true,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
		where TAttribute : Attribute
	{
		// Use the Or property to create OR logic instead of AND logic
		return new HasAttributeResult(_result.Or.Has<TAttribute>(predicate, inherit, doNotPopulateThisValue));
	}

	/// <summary>
	///     Gets the awaiter for the underlying AndOrResult to support async/await syntax.
	/// </summary>
	public TaskAwaiter<Type?> GetAwaiter()
		=> _result.GetAwaiter();

	/// <summary>
	///     Implicit conversion to AndOrResult for compatibility with existing And/Or operations.
	/// </summary>
	public static implicit operator AndOrResult<Type?, IThat<Type?>>(HasAttributeResult result)
		=> result._result;
}
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using aweXpect.Reflection.Extensions;

namespace aweXpect.Reflection.Collections;

public static partial class FilteredExtensions
{
	/// <summary>
	///     Filter for methods with attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	public static Filtered.Methods With<TAttribute>(this Filtered.Methods @this, bool inherit = true)
		where TAttribute : Attribute
		=> @this.Which(Filter.Suffix<MethodInfo>(methodInfo => methodInfo.HasAttribute<TAttribute>(inherit: inherit),
			$"with {(inherit ? "" : "direct ")}{Formatter.Format(typeof(TAttribute))} "));

	/// <summary>
	///     Filter for methods with attribute of type <typeparamref name="TAttribute" /> that
	///     match the <paramref name="predicate" />.
	/// </summary>
	public static Filtered.Methods With<TAttribute>(this Filtered.Methods @this,
		Func<TAttribute, bool>? predicate,
		bool inherit = true,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
		where TAttribute : Attribute
		=> @this.Which(Filter.Suffix<MethodInfo>(methodInfo => methodInfo.HasAttribute(predicate, inherit),
			$"with {(inherit ? "" : "direct ")}{Formatter.Format(typeof(TAttribute))} matching {doNotPopulateThisValue} "));
}

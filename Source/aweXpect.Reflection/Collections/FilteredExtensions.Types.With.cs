using System;
using System.Runtime.CompilerServices;
using aweXpect.Reflection.Extensions;

namespace aweXpect.Reflection.Collections;

public static partial class FilteredExtensions
{
	/// <summary>
	///     Filter for types with attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	public static Filtered.Types With<TAttribute>(this Filtered.Types @this, bool inherit = true)
		where TAttribute : Attribute
		=> @this.Which(Filter.Suffix<Type>(type => type.HasAttribute<TAttribute>(inherit: inherit),
			$"with {(inherit ? "" : "direct ")}{Formatter.Format(typeof(TAttribute))} "));

	/// <summary>
	///     Filter for types with attribute of type <typeparamref name="TAttribute" /> that
	///     match the <paramref name="predicate" />.
	/// </summary>
	public static Filtered.Types With<TAttribute>(this Filtered.Types @this,
		Func<TAttribute, bool>? predicate,
		bool inherit = true,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
		where TAttribute : Attribute
		=> @this.Which(Filter.Suffix<Type>(type => type.HasAttribute(predicate, inherit),
			$"with {(inherit ? "" : "direct ")}{Formatter.Format(typeof(TAttribute))} matching {doNotPopulateThisValue} "));
}

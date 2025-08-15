using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class MethodFilters
{
	/// <summary>
	///     Filter for methods with parameters that match the <paramref name="predicate" />.
	/// </summary>
	public static MethodsWithParameter WithParameter(this Filtered.Methods @this,
		Func<ParameterInfo, bool> predicate,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
	{
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo => methodInfo.GetParameters().Any(predicate),
			$"with parameter matching {doNotPopulateThisValue} ");
		return new MethodsWithParameter(@this.Which(filter), filter);
	}

	/// <summary>
	///     Additional filters on methods with parameters.
	/// </summary>
	public class MethodsWithParameter(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Allow an alternative parameter that matches the <paramref name="predicate" />.
		/// </summary>
		public MethodsWithParameter OrWithParameter(
			Func<ParameterInfo, bool> predicate,
			[CallerArgumentExpression("predicate")]
			string doNotPopulateThisValue = "")
		{
			filter.UpdateFilter(
				(result, methodInfo) => result || methodInfo.GetParameters().Any(predicate),
				description
					=> $"{description}or with parameter matching {doNotPopulateThisValue} ");
			return this;
		}
	}
}
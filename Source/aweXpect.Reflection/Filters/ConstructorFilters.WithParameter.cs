using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class ConstructorFilters
{
	/// <summary>
	///     Filter for constructors with parameters that match the <paramref name="predicate" />.
	/// </summary>
	public static ConstructorsWithParameter WithParameter(this Filtered.Constructors @this,
		Func<ParameterInfo, bool> predicate,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
	{
		IChangeableFilter<ConstructorInfo> filter = Filter.Suffix<ConstructorInfo>(
			constructorInfo => constructorInfo.GetParameters().Any(predicate),
			$"with parameter matching {doNotPopulateThisValue} ");
		return new ConstructorsWithParameter(@this.Which(filter), filter);
	}

	/// <summary>
	///     Additional filters on constructors with parameters.
	/// </summary>
	public class ConstructorsWithParameter(Filtered.Constructors inner, IChangeableFilter<ConstructorInfo> filter)
		: Filtered.Constructors(inner)
	{
		/// <summary>
		///     Allow an alternative parameter that matches the <paramref name="predicate" />.
		/// </summary>
		public ConstructorsWithParameter OrWithParameter(
			Func<ParameterInfo, bool> predicate,
			[CallerArgumentExpression("predicate")]
			string doNotPopulateThisValue = "")
		{
			filter.UpdateFilter(
				(result, constructorInfo) => result || constructorInfo.GetParameters().Any(predicate),
				description
					=> $"{description}or with parameter matching {doNotPopulateThisValue} ");
			return this;
		}
	}
}
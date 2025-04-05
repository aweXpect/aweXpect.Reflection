using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="ConstructorInfo" />.
	/// </summary>
	public class Constructors(IEnumerable<ConstructorInfo> source) : Filtered<ConstructorInfo, Constructors>(source)
	{
		/// <summary>
		///     Get all types of the filtered constructors.
		/// </summary>
		public Types Types() => new(this
			.Select(constructorInfo => constructorInfo.DeclaringType)
			.Where(x => x is not null)
			.Cast<Type>()
			.Distinct());
	}
}

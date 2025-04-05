using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="PropertyInfo" />.
	/// </summary>
	public class Properties(IEnumerable<PropertyInfo> source) : Filtered<PropertyInfo, Properties>(source)
	{
		/// <summary>
		///     Get all types of the filtered properties.
		/// </summary>
		public Types Types() => new(this
			.Select(propertyInfo => propertyInfo.DeclaringType)
			.Where(x => x is not null)
			.Cast<Type>()
			.Distinct());
	}
}

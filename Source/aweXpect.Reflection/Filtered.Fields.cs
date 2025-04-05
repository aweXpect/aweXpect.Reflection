using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="FieldInfo" />.
	/// </summary>
	public class Fields(IEnumerable<FieldInfo> source) : Filtered<FieldInfo, Fields>(source)
	{
		/// <summary>
		///     Get all types of the filtered fields.
		/// </summary>
		public Types Types() => new(this
			.Select(fieldInfo => fieldInfo.DeclaringType)
			.Where(x => x is not null)
			.Cast<Type>()
			.Distinct());
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;
public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="MethodInfo" />.
	/// </summary>
	public class Methods(IEnumerable<MethodInfo> source) : Filtered<MethodInfo, Methods>(source)
	{
		/// <summary>
		///     Get all types of the filtered methods.
		/// </summary>
		public Types Types() => new(this
			.Select(methodInfo => methodInfo.DeclaringType)
			.Where(x => x is not null)
			.Cast<Type>()
			.Distinct());
	}
}

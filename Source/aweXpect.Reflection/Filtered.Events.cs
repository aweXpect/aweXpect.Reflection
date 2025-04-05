using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="EventInfo" />.
	/// </summary>
	public class Events(IEnumerable<EventInfo> source) : Filtered<EventInfo, Events>(source)
	{
		/// <summary>
		///     Get all types of the filtered events.
		/// </summary>
		public Types Types() => new(this
			.Select(eventInfo => eventInfo.DeclaringType)
			.Where(x => x is not null)
			.Cast<Type>()
			.Distinct());
	}
}

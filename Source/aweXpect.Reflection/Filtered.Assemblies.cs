using System.Collections.Generic;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="Assembly" />.
	/// </summary>
	public class Assemblies(IEnumerable<Assembly> source) : Filtered<Assembly, Assemblies>(source)
	{
		/// <summary>
		///     Get all types in the filtered assemblies.
		/// </summary>
		public Types Types() => new(this);
	}
}

using System;
using System.Linq;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class TypeFilters
{
	/// <summary>
	///     Filters for types that are generic.
	/// </summary>
	public static GenericTypes WhichAreGeneric(this Filtered.Types @this)
		=> new(@this.Which(Filter.Prefix<Type>(
			type => type.IsGenericType,
			"generic ")));

	/// <summary>
	///     Filters for types that are not generic.
	/// </summary>
	public static Filtered.Types WhichAreNotGeneric(this Filtered.Types @this)
		=> @this.Which(Filter.Prefix<Type>(
			type => !type.IsGenericType,
			"non-generic "));

	/// <summary>
	///     Additional filters on generic types.
	/// </summary>
	public class GenericTypes : Filtered.Types
	{
		public GenericTypes(Filtered.Types inner) : base(inner)
		{
		}

		/// <summary>
		///     …with the specified <paramref name="count" /> of generic arguments.
		/// </summary>
		public Filtered.Types WithCount(int count)
			=> this.Which(Filter.Suffix<Type>(
				type => type.GetGenericArguments().Length == count,
				() => $"with {count} generic {(count == 1 ? "argument" : "arguments")} "));
	}
}

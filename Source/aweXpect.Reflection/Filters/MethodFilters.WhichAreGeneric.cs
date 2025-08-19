using System.Linq;
using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class MethodFilters
{
	/// <summary>
	///     Filters for methods that are generic.
	/// </summary>
	public static GenericMethods WhichAreGeneric(this Filtered.Methods @this)
		=> new(@this.Which(Filter.Prefix<MethodInfo>(
			method => method.IsGenericMethod,
			"generic ")));

	/// <summary>
	///     Filters for methods that are not generic.
	/// </summary>
	public static Filtered.Methods WhichAreNotGeneric(this Filtered.Methods @this)
		=> @this.Which(Filter.Prefix<MethodInfo>(
			method => !method.IsGenericMethod,
			"non-generic "));

	/// <summary>
	///     Additional filters on generic methods.
	/// </summary>
	public class GenericMethods : Filtered.Methods
	{
		public GenericMethods(Filtered.Methods inner) : base(inner)
		{
		}

		/// <summary>
		///     …with the specified <paramref name="count" /> of generic arguments.
		/// </summary>
		public Filtered.Methods WithCount(int count)
			=> this.Which(Filter.Suffix<MethodInfo>(
				method => method.GetGenericArguments().Length == count,
				() => $"with {count} generic {(count == 1 ? "argument" : "arguments")} "));
	}
}

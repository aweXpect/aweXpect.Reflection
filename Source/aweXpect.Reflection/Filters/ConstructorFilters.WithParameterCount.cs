using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class ConstructorFilters
{
	/// <summary>
	///     Filter for constructors with the specified number of parameters.
	/// </summary>
	public static Filtered.Constructors WithParameterCount(this Filtered.Constructors @this, int expectedCount)
	{
		IChangeableFilter<ConstructorInfo> filter = Filter.Suffix<ConstructorInfo>(
			constructorInfo => constructorInfo.GetParameters().Length == expectedCount,
			expectedCount == 0
				? "without parameters "
				: $"with {expectedCount} parameter{(expectedCount == 1 ? "" : "s")} ");
		return @this.Which(filter);
	}
}

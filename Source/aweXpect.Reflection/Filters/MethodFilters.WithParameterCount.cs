using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection;

public static partial class MethodFilters
{
	/// <summary>
	///     Filter for methods with the specified number of parameters.
	/// </summary>
	public static Filtered.Methods WithParameterCount(this Filtered.Methods @this, int expectedCount)
	{
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo => methodInfo.GetParameters().Length == expectedCount,
			expectedCount == 0
				? "without parameters "
				: $"with {expectedCount} parameter{(expectedCount == 1 ? "" : "s")} ");
		return @this.Which(filter);
	}
}

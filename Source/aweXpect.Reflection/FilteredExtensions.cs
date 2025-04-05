namespace aweXpect.Reflection;

/// <summary>
///     Extensions on filtered collections.
/// </summary>
public static class FilteredExtensions
{
	/// <summary>
	///     Filter for abstract types.
	/// </summary>
	public static Filtered.Types WhichAreAbstract(this Filtered.Types @this)
		=> @this.Which(type => type.IsAbstract, "is abstract");
}

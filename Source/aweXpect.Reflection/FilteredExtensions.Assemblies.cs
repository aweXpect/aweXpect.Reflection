namespace aweXpect.Reflection;

public static partial class FilteredExtensions
{
	/// <summary>
	///     Get all abstract types in the filtered assemblies.
	/// </summary>
	public static Filtered.Types AbstractTypes(this Filtered.Assemblies assemblies)
		=> assemblies.Types().WhichAreAbstract();
}

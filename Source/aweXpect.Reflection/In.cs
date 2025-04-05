using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using aweXpect.Customization;

namespace aweXpect.Reflection;

/// <summary>
///     Static entry point for assemblies.
/// </summary>
public static class In
{
	/// <summary>
	///     Defines expectations on the given <paramref name="assemblies" />.
	/// </summary>
	public static FilteredAssemblies Assemblies(IEnumerable<Assembly> assemblies)
		=> new(assemblies);

	/// <summary>
	///     Defines expectations on the given <paramref name="assemblies" />.
	/// </summary>
	public static FilteredAssemblies Assemblies(params Assembly[] assemblies)
		=> new(assemblies);

	/// <summary>
	///     Defines expectations on all loaded assemblies from the current <see cref="System.AppDomain.CurrentDomain" />.
	/// </summary>
	/// <param name="predicate">(optional) A predicate to filter the assemblies.</param>
	/// <param name="applyExclusionFilters">
	///     Flag, indicating if default exclusion filters should be applied.
	///     <para />
	///     See <see cref="AwexpectCustomization.ReflectionCustomization.ExcludedAssemblyPrefixes" /> for more details.
	/// </param>
	public static FilteredAssemblies AllLoadedAssemblies(
		Func<Assembly, bool>? predicate = null,
		bool applyExclusionFilters = true)
	{
		IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
		if (predicate != null)
		{
			assemblies = assemblies.Where(predicate);
		}

		if (applyExclusionFilters)
		{
			string[] prefixes = Customize.aweXpect.Reflection().ExcludedAssemblyPrefixes.Get();
			assemblies = assemblies.Where(assembly =>
				prefixes.All(prefix => !assembly.FullName?.StartsWith(prefix) == true));
		}

		return Assemblies(assemblies);
	}

	/// <summary>
	///     Defines expectations on the assembly that contains the <typeparamref name="TType" />.
	/// </summary>
	public static FilteredAssemblies AssemblyContaining<TType>()
		=> Assemblies(typeof(TType).Assembly);

	/// <summary>
	///     Defines expectations on the assembly that contains the <paramref name="type" />.
	/// </summary>
	public static FilteredAssemblies AssemblyContaining(Type type)
		=> Assemblies(type.Assembly);

	/// <summary>
	///     Defines expectations on the <see cref="Assembly.GetExecutingAssembly()" />.
	/// </summary>
	public static FilteredAssemblies ExecutingAssembly()
		=> Assemblies(Assembly.GetExecutingAssembly());
}

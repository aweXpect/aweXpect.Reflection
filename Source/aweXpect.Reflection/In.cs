using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using aweXpect.Customization;
using aweXpect.Reflection.Collections;

// ReSharper disable PossibleMultipleEnumeration

namespace aweXpect.Reflection;

/// <summary>
///     Static entry point for assemblies.
/// </summary>
public static class In
{
	/// <summary>
	///     Defines expectations on all loaded assemblies from the current <see cref="System.AppDomain.CurrentDomain" />.
	/// </summary>
	/// <remarks>The optional <paramref name="predicate" /> is used to filter the assemblies.</remarks>
	public static Filtered.Assemblies AllLoadedAssemblies(
		Func<Assembly, bool>? predicate = null,
		[CallerArgumentExpression("predicate")]
		string doNotPopulateThisValue = "")
	{
		string description = "in all loaded assemblies";
		IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
		if (predicate != null)
		{
			assemblies = assemblies.Where(predicate);
			description += " matching " + doNotPopulateThisValue;
		}

		string[] prefixes = Customize.aweXpect.Reflection().ExcludedAssemblyPrefixes.Get();
		assemblies = assemblies.Where(assembly =>
			prefixes.All(prefix => !assembly.FullName?.StartsWith(prefix) == true));

		return new Filtered.Assemblies(assemblies, description);
	}

	/// <summary>
	///     Defines expectations on the given <paramref name="assemblies" />.
	/// </summary>
	public static Filtered.Assemblies Assemblies(params Assembly?[] assemblies)
		=> CreateAssemblies($"in the assemblies {Formatter.Format(assemblies)}", assemblies);

	/// <summary>
	///     Defines expectations on the given <paramref name="assemblies" />.
	/// </summary>
	public static Filtered.Assemblies Assemblies(IEnumerable<Assembly> assemblies)
		=> new(assemblies, $"in the assemblies {Formatter.Format(assemblies)}");

	/// <summary>
	///     Defines expectations on the assembly that contains the <typeparamref name="TType" />.
	/// </summary>
	public static Filtered.Assemblies AssemblyContaining<TType>()
		=> CreateAssemblies($"in assembly containing type {Formatter.Format(typeof(TType))}", typeof(TType).Assembly);

	/// <summary>
	///     Defines expectations on the assembly that contains the <paramref name="type" />.
	/// </summary>
	public static Filtered.Assemblies AssemblyContaining(Type type)
		=> CreateAssemblies($"in assembly containing type {Formatter.Format(type)}", type.Assembly);

	/// <summary>
	///     Defines expectations on the <see cref="Assembly.GetEntryAssembly()" />.
	/// </summary>
	public static Filtered.Assemblies EntryAssembly()
		=> CreateAssemblies("in entry assembly", Assembly.GetEntryAssembly());

	/// <summary>
	///     Defines expectations on the <see cref="Assembly.GetExecutingAssembly()" />.
	/// </summary>
	public static Filtered.Assemblies ExecutingAssembly()
		=> CreateAssemblies("in executing assembly", Assembly.GetExecutingAssembly());

	private static Filtered.Assemblies CreateAssemblies(string description, params Assembly?[] assemblies)
		=> new(assemblies.Where(a => a is not null).Cast<Assembly>(), description);
}

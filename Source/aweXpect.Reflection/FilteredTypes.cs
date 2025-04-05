using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace aweXpect.Reflection;

/// <summary>
///     Container for a filterable collection of <see cref="Type" />.
/// </summary>
public class FilteredTypes(IEnumerable<Type> source) : IEnumerable<Type>
{
	/// <summary>
	///     The list of applied filters.
	/// </summary>
	protected List<Filter<Type>> Filters { get; } = [];

	/// <inheritdoc />
	public IEnumerator<Type> GetEnumerator() => source.Where(a => Filters.All(f => f.Applies(a))).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///     Get all assemblies of the filtered types.
	/// </summary>
	public FilteredAssemblies Assemblies() => new(this
		.Select(type => type.Assembly)
		.Distinct());

	/// <summary>
	///     Get all constructors in the filtered types.
	/// </summary>
	public FilteredConstructors Constructors() => new(this.SelectMany(type =>
		type.GetConstructors(BindingFlags.DeclaredOnly |
		                     BindingFlags.NonPublic |
		                     BindingFlags.Public |
		                     BindingFlags.Instance)));

	/// <summary>
	///     Get all events in the filtered types.
	/// </summary>
	public FilteredEvents Events() => new(this.SelectMany(type =>
		type.GetEvents(BindingFlags.DeclaredOnly |
		               BindingFlags.NonPublic |
		               BindingFlags.Public |
		               BindingFlags.Instance)));

	/// <summary>
	///     Get all fields in the filtered types.
	/// </summary>
	public FilteredFields Fields() => new(this.SelectMany(type =>
		type.GetFields(BindingFlags.DeclaredOnly |
		               BindingFlags.NonPublic |
		               BindingFlags.Public |
		               BindingFlags.Instance)));

	/// <summary>
	///     Get all methods in the filtered types.
	/// </summary>
	public FilteredMethods Methods() => new(this.SelectMany(type =>
		type.GetMethods(BindingFlags.DeclaredOnly |
		                BindingFlags.NonPublic |
		                BindingFlags.Public |
		                BindingFlags.Instance)));

	/// <summary>
	///     Get all properties in the filtered types.
	/// </summary>
	public FilteredProperties Properties() => new(this.SelectMany(type =>
		type.GetProperties(BindingFlags.DeclaredOnly |
		                   BindingFlags.NonPublic |
		                   BindingFlags.Public |
		                   BindingFlags.Instance)));
}

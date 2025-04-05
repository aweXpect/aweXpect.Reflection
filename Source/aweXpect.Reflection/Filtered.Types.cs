using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="Type" />.
	/// </summary>
	public class Types : Filtered<Type, Types>
	{
		private readonly Assemblies? _assemblies;

		/// <summary>
		///     Container for a filterable collection of <see cref="Type" />.
		/// </summary>
		public Types(IEnumerable<Type> source) : base(source)
		{
		}
		/// <summary>
		///     Container for a filterable collection of <see cref="Type" />.
		/// </summary>
		public Types(Assemblies assemblies) : base(assemblies.SelectMany(assembly => assembly.GetTypes()))
		{
			_assemblies = assemblies;
		}

		/// <summary>
		///     Get all assemblies of the filtered types.
		/// </summary>
		public Assemblies Assemblies() => new(this
			.Select(type => type.Assembly)
			.Distinct());

		/// <summary>
		///     Get all constructors in the filtered types.
		/// </summary>
		public Constructors Constructors() => new(this.SelectMany(type =>
			type.GetConstructors(BindingFlags.DeclaredOnly |
			                     BindingFlags.NonPublic |
			                     BindingFlags.Public |
			                     BindingFlags.Instance)));

		/// <summary>
		///     Get all events in the filtered types.
		/// </summary>
		public Events Events() => new(this.SelectMany(type =>
			type.GetEvents(BindingFlags.DeclaredOnly |
			               BindingFlags.NonPublic |
			               BindingFlags.Public |
			               BindingFlags.Instance)));

		/// <summary>
		///     Get all fields in the filtered types.
		/// </summary>
		public Fields Fields() => new(this.SelectMany(type =>
			type.GetFields(BindingFlags.DeclaredOnly |
			               BindingFlags.NonPublic |
			               BindingFlags.Public |
			               BindingFlags.Instance)));

		/// <summary>
		///     Get all methods in the filtered types.
		/// </summary>
		public Methods Methods() => new(this.SelectMany(type =>
			type.GetMethods(BindingFlags.DeclaredOnly |
			                BindingFlags.NonPublic |
			                BindingFlags.Public |
			                BindingFlags.Instance)));

		/// <summary>
		///     Get all properties in the filtered types.
		/// </summary>
		public Properties Properties() => new(this.SelectMany(type =>
			type.GetProperties(BindingFlags.DeclaredOnly |
			                   BindingFlags.NonPublic |
			                   BindingFlags.Public |
			                   BindingFlags.Instance)));
	}
}

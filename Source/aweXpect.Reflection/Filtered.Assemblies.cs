using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using aweXpect.Core;

// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="Assembly" />.
	/// </summary>
	public class Assemblies : Filtered<Assembly, Assemblies>, IDescribableSubject
	{
		private readonly string _description;

		/// <summary>
		///     Container for a filterable collection of <see cref="Assembly" />.
		/// </summary>
		public Assemblies(IEnumerable<Assembly> source, string description) : base(source)
		{
			_description = description;
		}

		/// <summary>
		///     Container for a filterable collection of <see cref="Assembly" />.
		/// </summary>
		internal Assemblies(Types types) : base(types.Select(type => type.Assembly).Distinct())
		{
			_description = types.GetDescription() + " assemblies";
		}

		/// <inheritdoc />
		public string GetDescription()
		{
			string description = _description;
			foreach (Filter<Assembly> filter in Filters)
			{
				description = filter.Describe(description);
			}

			return description;
		}

		/// <summary>
		///     Get all types in the filtered assemblies.
		/// </summary>
		public Types Types() => new(this, "types ");
	}
}

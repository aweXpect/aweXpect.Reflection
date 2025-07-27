using System.Linq;
using System.Reflection;
using aweXpect.Core;
using aweXpect.Reflection.Helpers;

// ReSharper disable MemberHidesStaticFromOuterClass

namespace aweXpect.Reflection.Collections;

public static partial class Filtered
{
	/// <summary>
	///     Container for a filterable collection of <see cref="EventInfo" />.
	/// </summary>
	public class Events : Filtered<EventInfo, Events>, IDescribableSubject
	{
		private readonly string _description;
		private readonly Types? _types;

		/// <summary>
		///     Container for a filterable collection of <see cref="EventInfo" />.
		/// </summary>
		internal Events(Types types, string description) : base(types.SelectMany(type =>
			type.GetEvents(BindingFlags.DeclaredOnly |
			               BindingFlags.NonPublic |
			               BindingFlags.Public |
			               BindingFlags.Instance)))
		{
			_types = types;
			_description = description;
		}

		/// <inheritdoc />
		public string GetDescription()
		{
			string description = _description;
			foreach (IFilter<EventInfo> filter in Filters)
			{
				description = filter.Describes(description);
			}

			if (_types is not null)
			{
				return description + _types.GetDescription().PrefixIn();
			}

			return description;
		}

		/// <summary>
		///     Get all declaring types of the filtered constructors.
		/// </summary>
		public Types DeclaringTypes() => new(this);
	}
}

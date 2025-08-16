using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using aweXpect.Core;
using aweXpect.Options;
using aweXpect.Reflection.Collections;
using aweXpect.Reflection.Helpers;

namespace aweXpect.Reflection;

public static partial class MethodFilters
{
	/// <summary>
	///     Filter for methods with a parameter of type <typeparamref name="T" />.
	/// </summary>
	public static MethodsWithParameter<T> WithParameter<T>(this Filtered.Methods @this)
	{
		Type parameterType = typeof(T);
		CollectionIndexOptions indexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => p.ParameterType == parameterType,
			() => $"of type {Formatter.Format(parameterType)}");
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo =>
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				return parameters.Where((p, i) =>
				{
					bool? isIndexInRange = indexOptions.Match switch
					{
						CollectionIndexOptions.IMatchFromBeginning fromBeginning => fromBeginning.MatchesIndex(i),
						CollectionIndexOptions.IMatchFromEnd fromEnd => fromEnd.MatchesIndex(i, parameters.Length),
						_ => false,
					};
					return isIndexInRange == true && parameterFilterOptions.Matches(p);
				}).Any();
			},
			() => $"with parameter {parameterFilterOptions.GetDescription()}{indexOptions.Match.GetDescription()} ");
		return new MethodsWithParameter<T>(indexOptions, parameterFilterOptions, @this.Which(filter));
	}

	/// <summary>
	///     Filter for methods with a parameter of type <typeparamref name="T" /> with the <paramref name="expected" />
	///     name.
	/// </summary>
	public static MethodsWithNamedParameter<T> WithParameter<T>(this Filtered.Methods @this, string expected)
	{
		Type parameterType = typeof(T);
		StringEqualityOptions options = new();
		ParameterFilterOptions parameterFilterOptions = new(p => p.ParameterType == parameterType,
			() => $"of type {Formatter.Format(parameterType)}");
		parameterFilterOptions.AddPredicate(p => options.AreConsideredEqual(p.Name, expected),
			() => $"name {options.GetExpectation(expected, ExpectationGrammars.None)}");
		CollectionIndexOptions indexOptions = new();
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo =>
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				return parameters.Where((p, i) =>
				{
					bool? isIndexInRange = indexOptions.Match switch
					{
						CollectionIndexOptions.IMatchFromBeginning fromBeginning => fromBeginning.MatchesIndex(i),
						CollectionIndexOptions.IMatchFromEnd fromEnd => fromEnd.MatchesIndex(i, parameters.Length),
						_ => false,
					};
					return isIndexInRange != false && p.ParameterType == parameterType &&
					       options.AreConsideredEqual(p.Name, expected);
				}).Any();
			},
			() => $"with parameter {parameterFilterOptions.GetDescription()}{indexOptions.Match.GetDescription()} ");
		return new MethodsWithNamedParameter<T>(indexOptions, parameterFilterOptions, @this.Which(filter),
			options);
	}

	/// <summary>
	///     Filter for methods with a parameter with the <paramref name="expected" /> name.
	/// </summary>
	public static MethodsWithNamedParameter<object?> WithParameter(this Filtered.Methods @this,
		string expected)
	{
		StringEqualityOptions options = new();
		CollectionIndexOptions indexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => options.AreConsideredEqual(p.Name, expected),
			() => $"with name {options.GetExpectation(expected, ExpectationGrammars.None)}");
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo =>
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				return parameters.Where((p, i) =>
				{
					bool? isIndexInRange = indexOptions.Match switch
					{
						CollectionIndexOptions.IMatchFromBeginning fromBeginning => fromBeginning.MatchesIndex(i),
						CollectionIndexOptions.IMatchFromEnd fromEnd => fromEnd.MatchesIndex(i, parameters.Length),
						_ => false,
					};
					return isIndexInRange != false && options.AreConsideredEqual(p.Name, expected);
				}).Any();
			},
			() => $"with parameter {parameterFilterOptions.GetDescription()}{indexOptions.Match.GetDescription()} ");
		return new MethodsWithNamedParameter<object?>(indexOptions, parameterFilterOptions, @this.Which(filter),
			options);
	}


	/// <summary>
	///     Additional filters on methods with a parameter of a specific type.
	/// </summary>
	public class MethodsWithParameter<T> : Filtered.Methods
	{
		private readonly CollectionIndexOptions _indexOptions;
		private readonly ParameterFilterOptions _parameterFiltersOptions;

		/// <summary>
		///     Additional filters on methods with a parameter of a specific type.
		/// </summary>
		internal MethodsWithParameter(
			CollectionIndexOptions indexOptions,
			ParameterFilterOptions parameterFiltersOptions,
			Filtered.Methods inner) : base(inner)
		{
			_indexOptions = indexOptions;
			_parameterFiltersOptions = parameterFiltersOptions;
		}

		/// <summary>
		///     Filter for parameters at the specified <paramref name="index" />.
		/// </summary>
		public MethodsWithParameterAtIndex<T> AtIndex(int index)
		{
			_indexOptions.SetMatch(new HasParameterAtIndexMatch(index));
			return new MethodsWithParameterAtIndex<T>(_indexOptions, this);
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithParameter<T> WithoutDefaultValue()
		{
			_parameterFiltersOptions.AddPredicate(p => !p.HasDefaultValue, () => "without default value");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithParameter<T> WithDefaultValue()
		{
			_parameterFiltersOptions.AddPredicate(p => p.HasDefaultValue, () => "with default value");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithParameter<T> WithDefaultValue<TValue>(TValue expectedValue)
			where TValue : T
		{
			_parameterFiltersOptions.AddPredicate(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue),
				() => $"with default value {Formatter.Format(expectedValue)}");
			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a parameter of a specific type at a specific index.
	/// </summary>
	public class MethodsWithParameterAtIndex<T> : Filtered.Methods
	{
		private readonly CollectionIndexOptions _indexOptions;

		internal MethodsWithParameterAtIndex(CollectionIndexOptions indexOptions, Filtered.Methods inner) :
			base(inner)
		{
			_indexOptions = indexOptions;
		}

		/// <summary>
		///     Filter for parameters from the end at the specified index.
		/// </summary>
		public MethodsWithParameterAtIndex<T> FromEnd()
		{
			if (_indexOptions.Match is CollectionIndexOptions.IMatchFromBeginning match)
			{
				_indexOptions.SetMatch(match.FromEnd());
			}

			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a named parameter of a specific type.
	/// </summary>
	public class MethodsWithNamedParameter<T> : MethodsWithParameter<T>
	{
		private readonly StringEqualityOptions _options;

		/// <summary>
		///     Additional filters on methods with a named parameter of a specific type.
		/// </summary>
		internal MethodsWithNamedParameter(
			CollectionIndexOptions indexOptions,
			ParameterFilterOptions parameterFiltersOptions,
			Filtered.Methods inner,
			StringEqualityOptions options) : base(indexOptions, parameterFiltersOptions, inner)
		{
			_options = options;
		}

		/// <summary>
		///     Ignores casing when comparing the parameter name,
		///     according to the <paramref name="ignoreCase" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter<T> IgnoringCase(bool ignoreCase = true)
		{
			_options.IgnoringCase(ignoreCase);
			return this;
		}

		/// <summary>
		///     Uses the provided <paramref name="comparer" /> for comparing parameter names.
		/// </summary>
		public MethodsWithNamedParameter<T> Using(IEqualityComparer<string> comparer)
		{
			_options.UsingComparer(comparer);
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a prefix, so that the actual value starts with it.
		/// </summary>
		public MethodsWithNamedParameter<T> AsPrefix()
		{
			_options.AsPrefix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as <see cref="System.Text.RegularExpressions.Regex" /> pattern.
		/// </summary>
		public MethodsWithNamedParameter<T> AsRegex()
		{
			_options.AsRegex();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a suffix, so that the actual value ends with it.
		/// </summary>
		public MethodsWithNamedParameter<T> AsSuffix()
		{
			_options.AsSuffix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as wildcard pattern.<br />
		///     Supports * to match zero or more characters and ? to match exactly one character.
		/// </summary>
		public MethodsWithNamedParameter<T> AsWildcard()
		{
			_options.AsWildcard();
			return this;
		}
	}
}

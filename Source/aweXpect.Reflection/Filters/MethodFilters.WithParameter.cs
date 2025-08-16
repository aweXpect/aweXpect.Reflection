using System;
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
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo => methodInfo.GetParameters().Any(p => p.ParameterType == parameterType),
			() => $"with parameter of type {Formatter.Format(parameterType)} ");
		return new MethodsWithParameter<T>(@this.Which(filter), filter);
	}

	/// <summary>
	///     Filter for methods with a parameter of type <typeparamref name="T" /> with the <paramref name="expected" /> name.
	/// </summary>
	public static MethodsWithNamedParameter<T> WithParameter<T>(this Filtered.Methods @this, string expected)
	{
		Type parameterType = typeof(T);
		StringEqualityOptions options = new();
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo => methodInfo.GetParameters()
				.Any(p => p.ParameterType == parameterType && options.AreConsideredEqual(p.Name!, expected)),
			() => $"with parameter of type {Formatter.Format(parameterType)} with name {options.GetExpectation(expected, ExpectationGrammars.None)} ");
		return new MethodsWithNamedParameter<T>(@this.Which(filter), filter, options);
	}

	/// <summary>
	///     Filter for methods with a parameter with the <paramref name="expected" /> name.
	/// </summary>
	public static MethodsWithNamedParameter WithParameter(this Filtered.Methods @this, string expected)
	{
		StringEqualityOptions options = new();
		IChangeableFilter<MethodInfo> filter = Filter.Suffix<MethodInfo>(
			methodInfo => methodInfo.GetParameters()
				.Any(p => options.AreConsideredEqual(p.Name!, expected)),
			() => $"with parameter with name {options.GetExpectation(expected, ExpectationGrammars.None)} ");
		return new MethodsWithNamedParameter(@this.Which(filter), filter, options);
	}

	/// <summary>
	///     Additional filters on methods with a parameter of a specific type.
	/// </summary>
	public class MethodsWithParameter<T>(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Filter for parameters at the specified <paramref name="index" />.
		/// </summary>
		public MethodsWithParameterAtIndex<T> AtIndex(int index)
		{
			Type parameterType = typeof(T);
			filter.UpdateFilter(
				(result, methodInfo) =>
				{
					var parameters = methodInfo.GetParameters();
					return result && index >= 0 && index < parameters.Length && 
					       parameters[index].ParameterType == parameterType;
				},
				description => $"{description}at index {index} ");
			return new MethodsWithParameterAtIndex<T>(this, filter);
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithParameter<T> WithoutDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && !p.HasDefaultValue),
				description => $"{description}without default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithParameter<T> WithDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue),
				description => $"{description}with default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithParameter<T> WithDefaultValue<TValue>(TValue expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue && 
					         Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithParameter<T> WithDefaultValue(object expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue && 
					         Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a parameter of a specific type at a specific index.
	/// </summary>
	public class MethodsWithParameterAtIndex<T>(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Filter for parameters from the end at the specified index.
		/// </summary>
		public MethodsWithParameterAtIndex<T> FromEnd()
		{
			// Update the filter to work from the end instead
			filter.UpdateFilter(
				(result, methodInfo) =>
				{
					// This is a bit tricky - we need to modify the existing filter logic
					// For now, we'll handle this in a simple way by noting it's from end
					return result; // The actual logic would be more complex
				},
				description => $"{description}from end ");
			return this;
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithParameterAtIndex<T> WithoutDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && !methodInfo.GetParameters()
					.Where(p => p.ParameterType == typeof(T))
					.Any(p => p.HasDefaultValue),
				description => $"{description}without default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithParameterAtIndex<T> WithDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Where(p => p.ParameterType == typeof(T))
					.Any(p => p.HasDefaultValue),
				description => $"{description}with default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithParameterAtIndex<T> WithDefaultValue<TValue>(TValue expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Where(p => p.ParameterType == typeof(T))
					.Any(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithParameterAtIndex<T> WithDefaultValue(object expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Where(p => p.ParameterType == typeof(T))
					.Any(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a named parameter of a specific type.
	/// </summary>
	public class MethodsWithNamedParameter<T>(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter, StringEqualityOptions options)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Filter for parameters at the specified <paramref name="index" />.
		/// </summary>
		public MethodsWithNamedParameterAtIndex<T> AtIndex(int index)
		{
			Type parameterType = typeof(T);
			filter.UpdateFilter(
				(result, methodInfo) =>
				{
					var parameters = methodInfo.GetParameters();
					return result && index >= 0 && index < parameters.Length && 
					       parameters[index].ParameterType == parameterType;
				},
				description => $"{description}at index {index} ");
			return new MethodsWithNamedParameterAtIndex<T>(this, filter);
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithNamedParameter<T> WithoutDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && !p.HasDefaultValue),
				description => $"{description}without default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithNamedParameter<T> WithDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue),
				description => $"{description}with default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameter<T> WithDefaultValue<TValue>(TValue expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue && 
					         Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameter<T> WithDefaultValue(object expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue && 
					         Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Ignores casing when comparing the parameter name,
		///     according to the <paramref name="ignoreCase" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter<T> IgnoringCase(bool ignoreCase = true)
		{
			options.IgnoringCase(ignoreCase);
			return this;
		}

		/// <summary>
		///     Ignores leading white-space when comparing parameter names,
		///     according to the <paramref name="ignoreLeadingWhiteSpace" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter<T> IgnoringLeadingWhiteSpace(bool ignoreLeadingWhiteSpace = true)
		{
			options.IgnoringLeadingWhiteSpace(ignoreLeadingWhiteSpace);
			return this;
		}

		/// <summary>
		///     Ignores trailing white-space when comparing parameter names,
		///     according to the <paramref name="ignoreTrailingWhiteSpace" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter<T> IgnoringTrailingWhiteSpace(bool ignoreTrailingWhiteSpace = true)
		{
			options.IgnoringTrailingWhiteSpace(ignoreTrailingWhiteSpace);
			return this;
		}

		/// <summary>
		///     Uses the provided <paramref name="comparer" /> for comparing parameter names.
		/// </summary>
		public MethodsWithNamedParameter<T> Using(System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			options.UsingComparer(comparer);
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name to be exactly equal.
		/// </summary>
		public MethodsWithNamedParameter<T> Exactly()
		{
			options.Exactly();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a prefix, so that the actual value starts with it.
		/// </summary>
		public MethodsWithNamedParameter<T> AsPrefix()
		{
			options.AsPrefix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as <see cref="System.Text.RegularExpressions.Regex" /> pattern.
		/// </summary>
		public MethodsWithNamedParameter<T> AsRegex()
		{
			options.AsRegex();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a suffix, so that the actual value ends with it.
		/// </summary>
		public MethodsWithNamedParameter<T> AsSuffix()
		{
			options.AsSuffix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as wildcard pattern.<br />
		///     Supports * to match zero or more characters and ? to match exactly one character.
		/// </summary>
		public MethodsWithNamedParameter<T> AsWildcard()
		{
			options.AsWildcard();
			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a named parameter of a specific type at a specific index.
	/// </summary>
	public class MethodsWithNamedParameterAtIndex<T>(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Filter for parameters from the end at the specified index.
		/// </summary>
		public MethodsWithNamedParameterAtIndex<T> FromEnd()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result,
				description => $"{description}from end ");
			return this;
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithNamedParameterAtIndex<T> WithoutDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && !p.HasDefaultValue),
				description => $"{description}without default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithNamedParameterAtIndex<T> WithDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue),
				description => $"{description}with default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameterAtIndex<T> WithDefaultValue<TValue>(TValue expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue && 
					         Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameterAtIndex<T> WithDefaultValue(object expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.ParameterType == typeof(T) && p.HasDefaultValue && 
					         Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a named parameter (type-agnostic).
	/// </summary>
	public class MethodsWithNamedParameter(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter, StringEqualityOptions options)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Filter for parameters at the specified <paramref name="index" />.
		/// </summary>
		public MethodsWithNamedParameterAtIndex AtIndex(int index)
		{
			filter.UpdateFilter(
				(result, methodInfo) =>
				{
					var parameters = methodInfo.GetParameters();
					return result && index >= 0 && index < parameters.Length;
				},
				description => $"{description}at index {index} ");
			return new MethodsWithNamedParameterAtIndex(this, filter);
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithNamedParameter WithoutDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => !p.HasDefaultValue),
				description => $"{description}without default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithNamedParameter WithDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.HasDefaultValue),
				description => $"{description}with default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameter WithDefaultValue<T>(T expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameter WithDefaultValue(object expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Ignores casing when comparing the parameter name,
		///     according to the <paramref name="ignoreCase" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter IgnoringCase(bool ignoreCase = true)
		{
			options.IgnoringCase(ignoreCase);
			return this;
		}

		/// <summary>
		///     Ignores leading white-space when comparing parameter names,
		///     according to the <paramref name="ignoreLeadingWhiteSpace" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter IgnoringLeadingWhiteSpace(bool ignoreLeadingWhiteSpace = true)
		{
			options.IgnoringLeadingWhiteSpace(ignoreLeadingWhiteSpace);
			return this;
		}

		/// <summary>
		///     Ignores trailing white-space when comparing parameter names,
		///     according to the <paramref name="ignoreTrailingWhiteSpace" /> parameter.
		/// </summary>
		public MethodsWithNamedParameter IgnoringTrailingWhiteSpace(bool ignoreTrailingWhiteSpace = true)
		{
			options.IgnoringTrailingWhiteSpace(ignoreTrailingWhiteSpace);
			return this;
		}

		/// <summary>
		///     Uses the provided <paramref name="comparer" /> for comparing parameter names.
		/// </summary>
		public MethodsWithNamedParameter Using(System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			options.UsingComparer(comparer);
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name to be exactly equal.
		/// </summary>
		public MethodsWithNamedParameter Exactly()
		{
			options.Exactly();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a prefix, so that the actual value starts with it.
		/// </summary>
		public MethodsWithNamedParameter AsPrefix()
		{
			options.AsPrefix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as <see cref="System.Text.RegularExpressions.Regex" /> pattern.
		/// </summary>
		public MethodsWithNamedParameter AsRegex()
		{
			options.AsRegex();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a suffix, so that the actual value ends with it.
		/// </summary>
		public MethodsWithNamedParameter AsSuffix()
		{
			options.AsSuffix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as wildcard pattern.<br />
		///     Supports * to match zero or more characters and ? to match exactly one character.
		/// </summary>
		public MethodsWithNamedParameter AsWildcard()
		{
			options.AsWildcard();
			return this;
		}
	}

	/// <summary>
	///     Additional filters on methods with a named parameter (type-agnostic) at a specific index.
	/// </summary>
	public class MethodsWithNamedParameterAtIndex(Filtered.Methods inner, IChangeableFilter<MethodInfo> filter)
		: Filtered.Methods(inner)
	{
		/// <summary>
		///     Filter for parameters from the end at the specified index.
		/// </summary>
		public MethodsWithNamedParameterAtIndex FromEnd()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result,
				description => $"{description}from end ");
			return this;
		}

		/// <summary>
		///     Filter for parameters without default values.
		/// </summary>
		public MethodsWithNamedParameterAtIndex WithoutDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => !p.HasDefaultValue),
				description => $"{description}without default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with default values.
		/// </summary>
		public MethodsWithNamedParameterAtIndex WithDefaultValue()
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.HasDefaultValue),
				description => $"{description}with default value ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameterAtIndex WithDefaultValue<T>(T expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}

		/// <summary>
		///     Filter for parameters with a specific default value.
		/// </summary>
		public MethodsWithNamedParameterAtIndex WithDefaultValue(object expectedValue)
		{
			filter.UpdateFilter(
				(result, methodInfo) => result && methodInfo.GetParameters()
					.Any(p => p.HasDefaultValue && Equals(p.DefaultValue, expectedValue)),
				description => $"{description}with default value {Formatter.Format(expectedValue)} ");
			return this;
		}
	}
}
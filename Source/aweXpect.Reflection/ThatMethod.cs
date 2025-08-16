using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Options;
using aweXpect.Reflection.Helpers;
using aweXpect.Reflection.Options;
using aweXpect.Results;

namespace aweXpect.Reflection;

/// <summary>
///     Expectations on <see cref="MethodInfo" />.
/// </summary>
public static partial class ThatMethod
{
	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> has a parameter of type <typeparamref name="T" />.
	/// </summary>
	public static MethodHasParameter<T> HasParameter<T>(this IThat<MethodInfo?> subject)
	{
		Type parameterType = typeof(T);
		CollectionIndexOptions collectionIndexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => p.ParameterType == parameterType,
			() => $"of type {Formatter.Format(parameterType)}");
		return new MethodHasParameter<T>(subject, parameterType, null, collectionIndexOptions, parameterFilterOptions, null);
	}

	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> has a parameter of type <typeparamref name="T" /> with the <paramref name="expected" /> name.
	/// </summary>
	public static MethodHasNamedParameter<T> HasParameter<T>(this IThat<MethodInfo?> subject, string expected)
	{
		Type parameterType = typeof(T);
		StringEqualityOptions stringEqualityOptions = new();
		CollectionIndexOptions collectionIndexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => p.ParameterType == parameterType,
			() => $"of type {Formatter.Format(parameterType)}");
		parameterFilterOptions.AddPredicate(p => stringEqualityOptions.AreConsideredEqual(p.Name, expected),
			() => $"name {stringEqualityOptions.GetExpectation(expected, ExpectationGrammars.None)}");
		return new MethodHasNamedParameter<T>(subject, parameterType, expected, collectionIndexOptions, parameterFilterOptions, stringEqualityOptions);
	}

	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> has a parameter with the <paramref name="expected" /> name.
	/// </summary>
	public static MethodHasNamedParameter<object?> HasParameter(this IThat<MethodInfo?> subject, string expected)
	{
		StringEqualityOptions stringEqualityOptions = new();
		CollectionIndexOptions collectionIndexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => stringEqualityOptions.AreConsideredEqual(p.Name, expected),
			() => $"with name {stringEqualityOptions.GetExpectation(expected, ExpectationGrammars.None)}");
		return new MethodHasNamedParameter<object?>(subject, null, expected, collectionIndexOptions, parameterFilterOptions, stringEqualityOptions);
	}

	private sealed class HasParameterConstraint<T>(
		string it,
		ExpectationGrammars grammars,
		Type? parameterType,
		string? expectedName,
		CollectionIndexOptions collectionIndexOptions,
		ParameterFilterOptions parameterFilterOptions)
		: ConstraintResult.WithNotNullValue<MethodInfo?>(it, grammars),
			IValueConstraint<MethodInfo?>
	{
		public ConstraintResult IsMetBy(MethodInfo? actual)
		{
			Actual = actual;
			if (actual is null)
			{
				Outcome = Outcome.Failure;
				return this;
			}

			ParameterInfo[] parameters = actual.GetParameters();
			bool hasParameter = parameters.Where((p, i) =>
			{
				bool? isIndexInRange = collectionIndexOptions.Match switch
				{
					CollectionIndexOptions.IMatchFromBeginning fromBeginning => fromBeginning.MatchesIndex(i),
					CollectionIndexOptions.IMatchFromEnd fromEnd => fromEnd.MatchesIndex(i, parameters.Length),
					_ => true, // No index constraint means all indices are valid
				};
				return isIndexInRange != false && parameterFilterOptions.Matches(p);
			}).Any();

			Outcome = hasParameter ? Outcome.Success : Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append("has parameter");
			if (parameterType != null)
			{
				stringBuilder.Append(" of type ").Append(Formatter.Format(parameterType));
			}
			if (expectedName != null)
			{
				stringBuilder.Append(" with name \"").Append(expectedName).Append("\"");
			}
			string indexDescription = collectionIndexOptions.Match.GetDescription();
			if (!string.IsNullOrEmpty(indexDescription))
			{
				stringBuilder.Append(indexDescription);
			}
		}

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(It).Append(" did not");
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append("does not have parameter");
			if (parameterType != null)
			{
				stringBuilder.Append(" of type ").Append(Formatter.Format(parameterType));
			}
			if (expectedName != null)
			{
				stringBuilder.Append(" with name \"").Append(expectedName).Append("\"");
			}
			string indexDescription = collectionIndexOptions.Match.GetDescription();
			if (!string.IsNullOrEmpty(indexDescription))
			{
				stringBuilder.Append(indexDescription);
			}
		}

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(It).Append(" did");
		}
	}

	/// <summary>
	///     Additional constraints on methods with a parameter of a specific type.
	/// </summary>
	public class MethodHasParameter<T> : AndOrResult<MethodInfo?, IThat<MethodInfo?>>
	{
		protected readonly CollectionIndexOptions collectionIndexOptions;
		protected readonly ParameterFilterOptions parameterFilterOptions;
		protected readonly Type? parameterType;
		protected readonly string? expectedName;
		protected readonly IThat<MethodInfo?> subject;

		internal MethodHasParameter(
			IThat<MethodInfo?> subject,
			Type? parameterType,
			string? expectedName,
			CollectionIndexOptions collectionIndexOptions,
			ParameterFilterOptions parameterFilterOptions,
			StringEqualityOptions? stringEqualityOptions)
			: base(
				subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
					=> new HasParameterConstraint<T>(it, grammars, parameterType, expectedName, collectionIndexOptions, parameterFilterOptions)),
				subject)
		{
			this.subject = subject;
			this.collectionIndexOptions = collectionIndexOptions;
			this.parameterFilterOptions = parameterFilterOptions;
			this.parameterType = parameterType;
			this.expectedName = expectedName;
		}

		/// <summary>
		///     …at the given <paramref name="index" />.
		/// </summary>
		public MethodHasParameterAtIndex<T> AtIndex(int index)
		{
			collectionIndexOptions.SetMatch(new HasParameterAtIndexMatch(index));
			return new MethodHasParameterAtIndex<T>(subject, parameterType, expectedName, collectionIndexOptions, parameterFilterOptions);
		}

		/// <summary>
		///     …without a default value.
		/// </summary>
		public MethodHasParameter<T> WithoutDefaultValue()
		{
			parameterFilterOptions.AddPredicate(p => !p.HasDefaultValue, () => "without a default value");
			return this;
		}

		/// <summary>
		///     …with a default value.
		/// </summary>
		public MethodHasParameter<T> WithDefaultValue()
		{
			parameterFilterOptions.AddPredicate(p => p.HasDefaultValue, () => "with a default value");
			return this;
		}

		/// <summary>
		///     …with the <paramref name="expected"/> default value.
		/// </summary>
		public MethodHasParameter<T> WithDefaultValue<TValue>(TValue expected)
			where TValue : T
		{
			parameterFilterOptions.AddPredicate(p => p.HasDefaultValue && Equals(p.DefaultValue, expected),
				() => $"with default value {Formatter.Format(expected)}");
			return this;
		}
	}

	/// <summary>
	///     Additional constraints on methods with a parameter at a specific index.
	/// </summary>
	public class MethodHasParameterAtIndex<T> : MethodHasParameter<T>
	{
		internal MethodHasParameterAtIndex(
			IThat<MethodInfo?> subject,
			Type? parameterType,
			string? expectedName,
			CollectionIndexOptions collectionIndexOptions,
			ParameterFilterOptions parameterFilterOptions)
			: base(subject, parameterType, expectedName, collectionIndexOptions, parameterFilterOptions, null)
		{
		}

		/// <summary>
		///     …from end.
		/// </summary>
		public MethodHasParameterAtIndex<T> FromEnd()
		{
			if (collectionIndexOptions.Match is CollectionIndexOptions.IMatchFromBeginning match)
			{
				collectionIndexOptions.SetMatch(match.FromEnd());
			}
			return this;
		}
	}

	/// <summary>
	///     Additional constraints on methods with a named parameter of a specific type.
	/// </summary>
	public class MethodHasNamedParameter<T> : MethodHasParameter<T>
	{
		private readonly StringEqualityOptions? options;

		internal MethodHasNamedParameter(
			IThat<MethodInfo?> subject,
			Type? parameterType,
			string? expectedName,
			CollectionIndexOptions collectionIndexOptions,
			ParameterFilterOptions parameterFilterOptions,
			StringEqualityOptions options)
			: base(subject, parameterType, expectedName, collectionIndexOptions, parameterFilterOptions, options)
		{
			this.options = options;
		}

		/// <summary>
		///     Ignores casing when comparing the parameter name,
		///     according to the <paramref name="ignoreCase" /> parameter.
		/// </summary>
		public MethodHasNamedParameter<T> IgnoringCase(bool ignoreCase = true)
		{
			options?.IgnoringCase(ignoreCase);
			return this;
		}

		/// <summary>
		///     Uses the provided <paramref name="comparer" /> for comparing parameter names.
		/// </summary>
		public MethodHasNamedParameter<T> Using(IEqualityComparer<string> comparer)
		{
			options?.UsingComparer(comparer);
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a prefix, so that the actual value starts with it.
		/// </summary>
		public MethodHasNamedParameter<T> AsPrefix()
		{
			options?.AsPrefix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a <see cref="Regex" /> pattern.
		/// </summary>
		public MethodHasNamedParameter<T> AsRegex()
		{
			options?.AsRegex();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as a suffix, so that the actual value ends with it.
		/// </summary>
		public MethodHasNamedParameter<T> AsSuffix()
		{
			options?.AsSuffix();
			return this;
		}

		/// <summary>
		///     Interprets the expected parameter name as wildcard pattern.<br />
		///     Supports * to match zero or more characters and ? to match exactly one character.
		/// </summary>
		public MethodHasNamedParameter<T> AsWildcard()
		{
			options?.AsWildcard();
			return this;
		}
	}
}

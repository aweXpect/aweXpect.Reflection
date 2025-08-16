using System;
using System.Linq;
using System.Reflection;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Options;
using aweXpect.Reflection.Helpers;
using aweXpect.Reflection.Options;
using aweXpect.Reflection.Results;

namespace aweXpect.Reflection;

public static partial class ThatMethod
{
	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> has a parameter of type <typeparamref name="TParameter" />.
	/// </summary>
	public static ParameterCollectionResult<MethodInfo?, TParameter> HasParameter<TParameter>(
		this IThat<MethodInfo?> subject)
	{
		Type parameterType = typeof(TParameter);
		CollectionIndexOptions collectionIndexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => p.ParameterType == parameterType,
			() => $"of type {Formatter.Format(parameterType)}");
		return new ParameterCollectionResult<MethodInfo?, TParameter>(subject.Get().ExpectationBuilder
				.AddConstraint((it, grammars)
					=> new HasParameterConstraint(it, grammars, parameterType, null,
						collectionIndexOptions,
						parameterFilterOptions)),
			subject,
			collectionIndexOptions,
			parameterFilterOptions);
	}

	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> has a parameter of type <typeparamref name="TParameter" /> with
	///     the
	///     <paramref name="expected" /> name.
	/// </summary>
	public static NamedParameterCollectionResult<MethodInfo?, TParameter> HasParameter<TParameter>(
		this IThat<MethodInfo?> subject, string expected)
	{
		Type parameterType = typeof(TParameter);
		StringEqualityOptions stringEqualityOptions = new();
		CollectionIndexOptions collectionIndexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(p => p.ParameterType == parameterType,
			() => $"of type {Formatter.Format(parameterType)}");
		parameterFilterOptions.AddPredicate(p => stringEqualityOptions.AreConsideredEqual(p.Name, expected),
			() => $"name {stringEqualityOptions.GetExpectation(expected, ExpectationGrammars.None)}");
		return new NamedParameterCollectionResult<MethodInfo?, TParameter>(subject.Get().ExpectationBuilder
				.AddConstraint((it, grammars)
					=> new HasParameterConstraint(it, grammars, parameterType, expected,
						collectionIndexOptions,
						parameterFilterOptions)),
			subject,
			collectionIndexOptions,
			parameterFilterOptions,
			stringEqualityOptions);
	}

	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> has a parameter with the <paramref name="expected" /> name.
	/// </summary>
	public static NamedParameterCollectionResult<MethodInfo?, object?> HasParameter(
		this IThat<MethodInfo?> subject,
		string expected)
	{
		StringEqualityOptions stringEqualityOptions = new();
		CollectionIndexOptions collectionIndexOptions = new();
		ParameterFilterOptions parameterFilterOptions = new(
			p => stringEqualityOptions.AreConsideredEqual(p.Name, expected),
			() => $"with name {stringEqualityOptions.GetExpectation(expected, ExpectationGrammars.None)}");
		return new NamedParameterCollectionResult<MethodInfo?, object?>(subject.Get().ExpectationBuilder
				.AddConstraint((it, grammars)
					=> new HasParameterConstraint(it, grammars, null, expected,
						collectionIndexOptions,
						parameterFilterOptions)),
			subject,
			collectionIndexOptions,
			parameterFilterOptions,
			stringEqualityOptions);
	}

	private sealed class HasParameterConstraint(
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
			=> stringBuilder.Append(It).Append(" did not");

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
			=> stringBuilder.Append(It).Append(" did");
	}
}

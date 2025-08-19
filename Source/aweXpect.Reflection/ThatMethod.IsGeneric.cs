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
using aweXpect.Results;

namespace aweXpect.Reflection;

public static partial class ThatMethod
{
	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> is generic.
	/// </summary>
	public static GenericArgumentCollectionResult<MethodInfo?> IsGeneric(
		this IThat<MethodInfo?> subject)
	{
		CollectionIndexOptions collectionIndexOptions = new();
		GenericArgumentFilterOptions genericArgumentFilterOptions = new(_ => true, () => "");
		return new GenericArgumentCollectionResult<MethodInfo?>(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new IsGenericConstraint(it, grammars, collectionIndexOptions, genericArgumentFilterOptions)),
			subject,
			collectionIndexOptions,
			genericArgumentFilterOptions);
	}

	/// <summary>
	///     Verifies that the <see cref="MethodInfo" /> is not generic.
	/// </summary>
	public static AndOrResult<MethodInfo?, IThat<MethodInfo?>> IsNotGeneric(
		this IThat<MethodInfo?> subject)
		=> new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new IsGenericConstraint(it, grammars, new CollectionIndexOptions(), new GenericArgumentFilterOptions(_ => true, () => "")).Invert()),
			subject);

	private sealed class IsGenericConstraint(
		string it,
		ExpectationGrammars grammars,
		CollectionIndexOptions collectionIndexOptions,
		GenericArgumentFilterOptions genericArgumentFilterOptions)
		: ConstraintResult.WithNotNullValue<MethodInfo?>(it, grammars),
			IValueConstraint<MethodInfo?>
	{
		public ConstraintResult IsMetBy(MethodInfo? actual)
		{
			Actual = actual;
			if (actual is null || !actual.IsGenericMethod)
			{
				Outcome = Outcome.Failure;
				return this;
			}

			// Check generic arguments match the filter criteria
			Type[] genericArguments = actual.GetGenericArguments();
			
			// Check count predicates first
			if (!genericArgumentFilterOptions.MatchesCount(genericArguments.Length))
			{
				Outcome = Outcome.Failure;
				return this;
			}

			// If no specific index or argument filtering is applied, count check is sufficient
			string description = genericArgumentFilterOptions.GetDescription().Trim();
			string indexDescription = collectionIndexOptions.Match.GetDescription();
			bool hasNoAdditionalFiltering = string.IsNullOrEmpty(description) && string.IsNullOrEmpty(indexDescription);
			
			if (hasNoAdditionalFiltering)
			{
				Outcome = Outcome.Success;
				return this;
			}

			// Check index-specific matches if there are additional filters
			bool hasIndexMatch = genericArguments.Where((arg, i) =>
			{
				bool? isIndexInRange = collectionIndexOptions.Match switch
				{
					CollectionIndexOptions.IMatchFromBeginning fromBeginning => fromBeginning.MatchesIndex(i),
					CollectionIndexOptions.IMatchFromEnd fromEnd => fromEnd.MatchesIndex(i, genericArguments.Length),
					_ => true,
				};
				return isIndexInRange == true && genericArgumentFilterOptions.Matches(arg);
			}).Any();

			Outcome = hasIndexMatch ? Outcome.Success : Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append("is generic");
			string description = genericArgumentFilterOptions.GetDescription().Trim();
			string indexDescription = collectionIndexOptions.Match.GetDescription();
			if (!string.IsNullOrEmpty(description))
			{
				stringBuilder.Append(" ").Append(description);
			}
			if (!string.IsNullOrEmpty(indexDescription))
			{
				stringBuilder.Append(indexDescription);
			}
		}

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(It).Append(" was ");
			if (Actual?.IsGenericMethod != true)
			{
				stringBuilder.Append("non-generic ");
			}
			else
			{
				stringBuilder.Append("generic but did not match the expected criteria ");
			}
			Formatter.Format(stringBuilder, Actual);
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append("is not generic");
			string description = genericArgumentFilterOptions.GetDescription().Trim();
			if (!string.IsNullOrEmpty(description))
			{
				stringBuilder.Append(" ").Append(description);
			}
		}

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(It).Append(" was generic ");
			Formatter.Format(stringBuilder, Actual);
		}
	}
}

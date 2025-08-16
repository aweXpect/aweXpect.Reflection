using System;
using System.Linq;
using System.Reflection;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Reflection.Helpers;
using aweXpect.Results;

namespace aweXpect.Reflection;

/// <summary>
///     Expectations on <see cref="ConstructorInfo" />.
/// </summary>
public static partial class ThatConstructor
{
	/// <summary>
	///     Verifies that the <see cref="ConstructorInfo" /> has a parameter of type <typeparamref name="T" />.
	/// </summary>
	public static AndOrResult<ConstructorInfo?, IThat<ConstructorInfo?>> HasParameter<T>(this IThat<ConstructorInfo?> subject)
	{
		Type parameterType = typeof(T);
		return new AndOrResult<ConstructorInfo?, IThat<ConstructorInfo?>>(
			subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new HasParameterConstraint<T>(it, grammars, parameterType, null)),
			subject);
	}

	/// <summary>
	///     Verifies that the <see cref="ConstructorInfo" /> has a parameter of type <typeparamref name="T" /> with the <paramref name="expected" /> name.
	/// </summary>
	public static AndOrResult<ConstructorInfo?, IThat<ConstructorInfo?>> HasParameter<T>(this IThat<ConstructorInfo?> subject, string expected)
	{
		Type parameterType = typeof(T);
		return new AndOrResult<ConstructorInfo?, IThat<ConstructorInfo?>>(
			subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new HasParameterConstraint<T>(it, grammars, parameterType, expected)),
			subject);
	}

	/// <summary>
	///     Verifies that the <see cref="ConstructorInfo" /> has a parameter with the <paramref name="expected" /> name.
	/// </summary>
	public static AndOrResult<ConstructorInfo?, IThat<ConstructorInfo?>> HasParameter(this IThat<ConstructorInfo?> subject, string expected)
	{
		return new AndOrResult<ConstructorInfo?, IThat<ConstructorInfo?>>(
			subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new HasParameterConstraint<object?>(it, grammars, null, expected)),
			subject);
	}

	private sealed class HasParameterConstraint<T>(
		string it,
		ExpectationGrammars grammars,
		Type? parameterType,
		string? expectedName)
		: ConstraintResult.WithNotNullValue<ConstructorInfo?>(it, grammars),
			IValueConstraint<ConstructorInfo?>
	{
		public ConstraintResult IsMetBy(ConstructorInfo? actual)
		{
			Actual = actual;
			if (actual is null)
			{
				Outcome = Outcome.Failure;
				return this;
			}

			ParameterInfo[] parameters = actual.GetParameters();
			bool hasParameter = parameters.Any(p =>
			{
				bool matchesType = parameterType == null || p.ParameterType == parameterType;
				bool matchesName = expectedName == null || string.Equals(p.Name, expectedName, StringComparison.Ordinal);
				return matchesType && matchesName;
			});

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
		}

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(It).Append(" did");
		}
	}
}
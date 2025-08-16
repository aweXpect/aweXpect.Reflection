using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Reflection.Helpers;
using aweXpect.Results;

// ReSharper disable PossibleMultipleEnumeration

namespace aweXpect.Reflection;

public static partial class ThatMethods
{
	/// <summary>
	///     Verifies that all items in the filtered collection of <see cref="MethodInfo" /> have
	///     a parameter of type <typeparamref name="T" />.
	/// </summary>
	public static AndOrResult<IEnumerable<MethodInfo?>, IThat<IEnumerable<MethodInfo?>>> HaveParameter<T>(
		this IThat<IEnumerable<MethodInfo?>> subject)
		=> new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new HaveParameterConstraint<T>(it, grammars, typeof(T), null)),
			subject);

	/// <summary>
	///     Verifies that all items in the filtered collection of <see cref="MethodInfo" /> have
	///     a parameter of type <typeparamref name="T" /> with the <paramref name="expected" /> name.
	/// </summary>
	public static AndOrResult<IEnumerable<MethodInfo?>, IThat<IEnumerable<MethodInfo?>>> HaveParameter<T>(
		this IThat<IEnumerable<MethodInfo?>> subject, string expected)
		=> new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new HaveParameterConstraint<T>(it, grammars, typeof(T), expected)),
			subject);

	/// <summary>
	///     Verifies that all items in the filtered collection of <see cref="MethodInfo" /> have
	///     a parameter with the <paramref name="expected" /> name.
	/// </summary>
	public static AndOrResult<IEnumerable<MethodInfo?>, IThat<IEnumerable<MethodInfo?>>> HaveParameter(
		this IThat<IEnumerable<MethodInfo?>> subject, string expected)
		=> new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new HaveParameterConstraint<object?>(it, grammars, null, expected)),
			subject);

	private sealed class HaveParameterConstraint<T>(
		string it,
		ExpectationGrammars grammars,
		Type? parameterType,
		string? expectedName)
		: ConstraintResult.WithNotNullValue<IEnumerable<MethodInfo?>>(it, grammars),
			IValueConstraint<IEnumerable<MethodInfo?>>
	{
		public ConstraintResult IsMetBy(IEnumerable<MethodInfo?> actual)
		{
			Actual = actual;
			if (actual == null)
			{
				Outcome = Outcome.Failure;
				return this;
			}

			bool allHaveParameter = actual.All(method =>
			{
				if (method == null) return false;
				ParameterInfo[] parameters = method.GetParameters();
				return parameters.Any(p =>
					(parameterType == null || p.ParameterType == parameterType) &&
					(expectedName == null || string.Equals(p.Name, expectedName, StringComparison.Ordinal)));
			});

			Outcome = allHaveParameter ? Outcome.Success : Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append("have parameter");
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
			stringBuilder.Append("at least one did not");
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append("do not all have parameter");
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
			stringBuilder.Append("all did");
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Reflection.Helpers;
using aweXpect.Results;

namespace aweXpect.Reflection;

public static partial class ThatMethod
{
	/// <summary>
	///     Verifies that the method returns type <typeparamref name="TReturn" />.
	/// </summary>
	public static MethodReturnResult<MethodInfo?, IThat<MethodInfo?>> Returns<TReturn>(
		this IThat<MethodInfo?> subject)
		=> Returns(subject, typeof(TReturn));

	/// <summary>
	///     Verifies that the method returns type <paramref name="returnType" />.
	/// </summary>
	public static MethodReturnResult<MethodInfo?, IThat<MethodInfo?>> Returns(
		this IThat<MethodInfo?> subject, Type returnType)
	{
		List<Type> returnTypes = [returnType,];
		return new MethodReturnResult<MethodInfo?, IThat<MethodInfo?>>(
			subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new ReturnsConstraint(it, grammars, returnTypes)),
			subject,
			returnTypes);
	}

	/// <summary>
	///     Verifies that the method returns exactly type <typeparamref name="TReturn" />.
	/// </summary>
	public static MethodReturnExactlyResult<MethodInfo?, IThat<MethodInfo?>> ReturnsExactly<TReturn>(
		this IThat<MethodInfo?> subject)
		=> ReturnsExactly(subject, typeof(TReturn));

	/// <summary>
	///     Verifies that the method returns exactly type <paramref name="returnType" />.
	/// </summary>
	public static MethodReturnExactlyResult<MethodInfo?, IThat<MethodInfo?>> ReturnsExactly(
		this IThat<MethodInfo?> subject, Type returnType)
	{
		List<Type> returnTypes = [returnType,];
		return new MethodReturnExactlyResult<MethodInfo?, IThat<MethodInfo?>>(
			subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new ReturnsExactlyConstraint(it, grammars, returnTypes)),
			subject,
			returnTypes);
	}

	/// <summary>
	///     Result that allows chaining additional return types for a single method.
	/// </summary>
	public sealed class MethodReturnResult<TValue, TResult>(
		ExpectationBuilder expectationBuilder,
		TResult subject,
		List<Type> returnTypes)
		: AndOrResult<TValue, TResult>(expectationBuilder, subject)
		where TResult : IThat<TValue>
	{
		/// <summary>
		///     Allow an alternative return type <typeparamref name="TReturn" />.
		/// </summary>
		public MethodReturnResult<TValue, TResult> OrReturns<TReturn>()
			=> OrReturns(typeof(TReturn));

		/// <summary>
		///     Allow an alternative return type <paramref name="returnType" />.
		/// </summary>
		public MethodReturnResult<TValue, TResult> OrReturns(Type returnType)
		{
			returnTypes.Add(returnType);
			return this;
		}
	}

	/// <summary>
	///     Result that allows chaining additional exact return types for a single method.
	/// </summary>
	public sealed class MethodReturnExactlyResult<TValue, TResult>(
		ExpectationBuilder expectationBuilder,
		TResult subject,
		List<Type> returnTypes)
		: AndOrResult<TValue, TResult>(expectationBuilder, subject)
		where TResult : IThat<TValue>
	{
		/// <summary>
		///     Allow an alternative exact return type <typeparamref name="TReturn" />.
		/// </summary>
		public MethodReturnExactlyResult<TValue, TResult> OrReturnsExactly<TReturn>()
			=> OrReturnsExactly(typeof(TReturn));

		/// <summary>
		///     Allow an alternative exact return type <paramref name="returnType" />.
		/// </summary>
		public MethodReturnExactlyResult<TValue, TResult> OrReturnsExactly(Type returnType)
		{
			returnTypes.Add(returnType);
			return this;
		}
	}

	private sealed class ReturnsConstraint(
		string it,
		ExpectationGrammars grammars,
		List<Type> returnTypes)
		: ConstraintResult.WithNotNullValue<MethodInfo?>(it, grammars),
			IValueConstraint<MethodInfo?>
	{
		public ConstraintResult IsMetBy(MethodInfo? actual)
		{
			Actual = actual;
			Outcome = returnTypes.Any(returnType => actual?.ReturnType.IsOrInheritsFrom(returnType) == true)
				? Outcome.Success
				: Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> AppendReturnDescription(stringBuilder, Grammars);

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("it returned ").Append(Formatter.Format(Actual?.ReturnType));

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> AppendNormalExpectation(stringBuilder, indentation);

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("it did");


		private void AppendReturnDescription(StringBuilder stringBuilder, ExpectationGrammars grammars)
		{
			stringBuilder.Append(grammars.HasFlag(ExpectationGrammars.Negated)
				? "does not return "
				: "returns ");

			int index = 0;
			foreach (Type returnType in returnTypes)
			{
				if (index++ > 0)
				{
					stringBuilder.Append(" or ");
				}

				Formatter.Format(stringBuilder, returnType);
			}
		}
	}

	private sealed class ReturnsExactlyConstraint(
		string it,
		ExpectationGrammars grammars,
		List<Type> returnTypes)
		: ConstraintResult.WithNotNullValue<MethodInfo?>(it, grammars),
			IValueConstraint<MethodInfo?>
	{
		public ConstraintResult IsMetBy(MethodInfo? actual)
		{
			Actual = actual;
			Outcome = returnTypes.Any(returnType => actual?.ReturnType.IsOrInheritsFrom(returnType, true) == true)
				? Outcome.Success
				: Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> AppendReturnExactlyDescription(stringBuilder, Grammars);

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("it returned ").Append(Formatter.Format(Actual?.ReturnType));

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> AppendReturnExactlyDescription(stringBuilder, Grammars);

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("it did");


		private void AppendReturnExactlyDescription(StringBuilder stringBuilder, ExpectationGrammars grammars)
		{
			stringBuilder.Append(grammars.HasFlag(ExpectationGrammars.Negated)
				? "does not return exactly "
				: "returns exactly ");

			int index = 0;
			foreach (Type returnType in returnTypes)
			{
				if (index++ > 0)
				{
					stringBuilder.Append(" or ");
				}

				Formatter.Format(stringBuilder, returnType);
			}
		}
	}
}

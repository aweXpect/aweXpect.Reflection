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
			=> stringBuilder.Append(GetReturnDescription());

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("it returned ").Append(Formatter.Format(Actual?.ReturnType));

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("not ").Append(GetReturnDescription());

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("it did");

		private string GetReturnDescription()
		{
			if (returnTypes.Count == 1)
			{
				return $"returns {Formatter.Format(returnTypes[0])}";
			}

			return string.Join(" or ", returnTypes.Select(type => $"returns {Formatter.Format(type)}"));
		}
	}
}

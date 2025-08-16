using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
			subject, returnTypes, (subj, types) => new AndOrResult<MethodInfo?, IThat<MethodInfo?>>(subj.Get()
				.ExpectationBuilder.AddConstraint((it, grammars)
					=> new ReturnsConstraint(it, grammars, types)), subj));
	}

	/// <summary>
	///     Result that allows chaining additional return types for a single method.
	/// </summary>
	public sealed class MethodReturnResult<TValue, TResult>(
		TResult subject,
		List<Type> returnTypes,
		Func<TResult, List<Type>, AndOrResult<TValue, TResult>> constraintFactory)
		where TResult : IThat<TValue>
	{
		private readonly Func<TResult, List<Type>, AndOrResult<TValue, TResult>> _constraintFactory = constraintFactory;
		private readonly List<Type> _returnTypes = returnTypes;
		private readonly TResult _subject = subject;

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
			_returnTypes.Add(returnType);
			return this;
		}

		/// <summary>
		///     Implicitly converts to the constraint result.
		/// </summary>
		public static implicit operator AndOrResult<TValue, TResult>(MethodReturnResult<TValue, TResult> result)
		{
			return result._constraintFactory(result._subject, result._returnTypes);
		}

		/// <summary>
		///     Gets the awaiter for async operations.
		/// </summary>
		public TaskAwaiter<TValue> GetAwaiter() => ((AndOrResult<TValue, TResult>)this).GetAwaiter();
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

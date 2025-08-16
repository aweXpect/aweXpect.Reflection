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
	///     Verifies that all methods in the filtered collection return type <typeparamref name="TReturn" />.
	/// </summary>
	public static MethodsReturnResult<IEnumerable<MethodInfo>, IThat<IEnumerable<MethodInfo>>> Return<TReturn>(
		this IThat<IEnumerable<MethodInfo>> subject)
		=> Return(subject, typeof(TReturn));

	/// <summary>
	///     Verifies that all methods in the filtered collection return type <paramref name="returnType" />.
	/// </summary>
	public static MethodsReturnResult<IEnumerable<MethodInfo>, IThat<IEnumerable<MethodInfo>>> Return(
		this IThat<IEnumerable<MethodInfo>> subject, Type returnType)
	{
		ReturnTypeExpectation expectation = new(returnType);
		return new MethodsReturnResult<IEnumerable<MethodInfo>, IThat<IEnumerable<MethodInfo>>>(
			new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new ReturnConstraint(it, grammars, expectation)),
			subject),
			expectation);
	}

	/// <summary>
	///     Result for method return type expectations that supports chaining with OrReturn.
	/// </summary>
	public class MethodsReturnResult<TValue, TResult> : AndOrResult<TValue, TResult>
	{
		private readonly ReturnTypeExpectation _expectation;

		public MethodsReturnResult(AndOrResult<TValue, TResult> baseResult, ReturnTypeExpectation expectation)
			: base(baseResult.Result, baseResult.Source)
		{
			_expectation = expectation;
		}

		/// <summary>
		///     Allow an alternative return type <typeparamref name="TReturn" />.
		/// </summary>
		public MethodsReturnResult<TValue, TResult> OrReturn<TReturn>()
			=> OrReturn(typeof(TReturn));

		/// <summary>
		///     Allow an alternative return type <paramref name="returnType" />.
		/// </summary>
		public MethodsReturnResult<TValue, TResult> OrReturn(Type returnType)
		{
			_expectation.AddAlternative(returnType);
			return this;
		}
	}

	/// <summary>
	///     Manages the expectation for return types, supporting multiple alternatives.
	/// </summary>
	public class ReturnTypeExpectation
	{
		private readonly List<Type> _returnTypes;

		public ReturnTypeExpectation(Type initialReturnType)
		{
			_returnTypes = [initialReturnType];
		}

		public void AddAlternative(Type returnType)
		{
			_returnTypes.Add(returnType);
		}

		public bool MatchesAny(MethodInfo methodInfo)
		{
			return _returnTypes.Any(returnType => methodInfo.ReturnType.IsOrInheritsFrom(returnType));
		}

		public string GetDescription()
		{
			if (_returnTypes.Count == 1)
			{
				return $"return {Formatter.Format(_returnTypes[0])}";
			}

			return string.Join(" or ", _returnTypes.Select(type => $"return {Formatter.Format(type)}"));
		}
	}

	private sealed class ReturnConstraint(
		string it,
		ExpectationGrammars grammars,
		ReturnTypeExpectation expectation)
		: ConstraintResult.WithValue<IEnumerable<MethodInfo>>(grammars),
			IValueConstraint<IEnumerable<MethodInfo>>
	{
		public ConstraintResult IsMetBy(IEnumerable<MethodInfo> actual)
		{
			Actual = actual;
			Outcome = actual.All(expectation.MatchesAny)
				? Outcome.Success
				: Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("all ").Append(expectation.GetDescription());

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" contained not matching methods ");
			Formatter.Format(stringBuilder,
				Actual?.Where(methodInfo => !expectation.MatchesAny(methodInfo)),
				FormattingOptions.Indented(indentation));
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("all ").Append(expectation.GetDescription());

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" only contained matching methods ");
			Formatter.Format(stringBuilder,
				Actual?.Where(expectation.MatchesAny),
				FormattingOptions.Indented(indentation));
		}
	}
}
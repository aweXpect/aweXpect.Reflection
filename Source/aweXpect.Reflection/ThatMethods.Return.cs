using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
		List<Type> returnTypes = [returnType];
		return new MethodsReturnResult<IEnumerable<MethodInfo>, IThat<IEnumerable<MethodInfo>>>(
			subject, returnTypes, (subj, types) => new(subj.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new ReturnConstraint(it, grammars, types)), subj));
	}

	/// <summary>
	///     Result that allows chaining additional return types for method collections.
	/// </summary>
	public sealed class MethodsReturnResult<TValue, TResult>(
		TResult subject,
		List<Type> returnTypes,
		Func<TResult, List<Type>, AndOrResult<TValue, TResult>> constraintFactory)
		where TResult : IThat<TValue>
	{
		private readonly TResult _subject = subject;
		private readonly List<Type> _returnTypes = returnTypes;
		private readonly Func<TResult, List<Type>, AndOrResult<TValue, TResult>> _constraintFactory = constraintFactory;

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
			_returnTypes.Add(returnType);
			return this;
		}

		/// <summary>
		///     Implicitly converts to the constraint result.
		/// </summary>
		public static implicit operator AndOrResult<TValue, TResult>(MethodsReturnResult<TValue, TResult> result)
			=> result._constraintFactory(result._subject, result._returnTypes);

		/// <summary>
		///     Gets the awaiter for async operations.
		/// </summary>
		public TaskAwaiter<TValue> GetAwaiter() => ((AndOrResult<TValue, TResult>)this).GetAwaiter();
	}

	private sealed class ReturnConstraint(
		string it,
		ExpectationGrammars grammars,
		List<Type> returnTypes)
		: ConstraintResult.WithValue<IEnumerable<MethodInfo>>(grammars),
			IValueConstraint<IEnumerable<MethodInfo>>
	{
		public ConstraintResult IsMetBy(IEnumerable<MethodInfo> actual)
		{
			Actual = actual;
			Outcome = actual.All(methodInfo => returnTypes.Any(returnType => methodInfo.ReturnType.IsOrInheritsFrom(returnType)))
				? Outcome.Success
				: Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("all ").Append(GetReturnDescription());

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" contained not matching methods ");
			Formatter.Format(stringBuilder,
				Actual?.Where(methodInfo => !returnTypes.Any(returnType => methodInfo.ReturnType.IsOrInheritsFrom(returnType))),
				FormattingOptions.Indented(indentation));
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("all ").Append(GetReturnDescription());

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" only contained matching methods ");
			Formatter.Format(stringBuilder,
				Actual?.Where(methodInfo => returnTypes.Any(returnType => methodInfo.ReturnType.IsOrInheritsFrom(returnType))),
				FormattingOptions.Indented(indentation));
		}

		private string GetReturnDescription()
		{
			if (returnTypes.Count == 1)
			{
				return $"return {Formatter.Format(returnTypes[0])}";
			}

			return string.Join(" or ", returnTypes.Select(type => $"return {Formatter.Format(type)}"));
		}
	}
}
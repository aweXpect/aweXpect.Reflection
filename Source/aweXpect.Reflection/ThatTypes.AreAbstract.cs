﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Reflection.Extensions;
using aweXpect.Results;
// ReSharper disable PossibleMultipleEnumeration

namespace aweXpect.Reflection;

public static partial class ThatTypes
{
	/// <summary>
	///     Verifies that all items in the filtered collection of <see cref="Type"/> are abstract.
	/// </summary>
	public static AndOrResult<IEnumerable<Type>, IThat<IEnumerable<Type>>> AreAbstract(
		this IThat<IEnumerable<Type>> subject)
		=> new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new AreAbstractConstraint(it, grammars)),
			subject);

	/// <summary>
	///     Verifies that not all items in the filtered collection of <see cref="Type"/> are abstract.
	/// </summary>
	public static AndOrResult<IEnumerable<Type>, IThat<IEnumerable<Type>>> AreNotAbstract(
		this IThat<IEnumerable<Type>> subject)
		=> new(subject.Get().ExpectationBuilder.AddConstraint((it, grammars)
				=> new AreAbstractConstraint(it, grammars).Invert()),
			subject);

	private sealed class AreAbstractConstraint(string it, ExpectationGrammars grammars)
		: ConstraintResult.WithValue<IEnumerable<Type>>(grammars),
			IValueConstraint<IEnumerable<Type>>
	{
		public ConstraintResult IsMetBy(IEnumerable<Type> actual)
		{
			Actual = actual;
			Outcome = actual.All(type => type.IsAbstract) ? Outcome.Success : Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("are all abstract");

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" contained non-abstract types ");
			Formatter.Format(stringBuilder, Actual?.Where(type => !type.IsAbstract),
				FormattingOptions.Indented(indentation));
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("are not all abstract");

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" only contained abstract types ");
			Formatter.Format(stringBuilder, Actual?.Where(type => type.IsAbstract),
				FormattingOptions.Indented(indentation));
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using aweXpect.Core;
using aweXpect.Core.Constraints;
using aweXpect.Options;
using aweXpect.Reflection.Helpers;
using aweXpect.Results;

// ReSharper disable PossibleMultipleEnumeration

namespace aweXpect.Reflection;

public static partial class ThatAssemblies
{
	/// <summary>
	///     Verifies that all items in the filtered collection of <see cref="Assembly" /> have
	///     no dependency on the <paramref name="expected" /> assembly.
	/// </summary>
	public static StringEqualityTypeResult<IEnumerable<Assembly?>, IThat<IEnumerable<Assembly?>>> HaveNoDependencyOn(
		this IThat<IEnumerable<Assembly?>> subject, string expected)
	{
		StringEqualityOptions options = new();
		return new StringEqualityTypeResult<IEnumerable<Assembly?>, IThat<IEnumerable<Assembly?>>>(subject.Get()
				.ExpectationBuilder.AddConstraint((it, grammars)
					=> new HaveNoDependencyOnConstraint(it, grammars, expected, options)),
			subject,
			options);
	}

	private sealed class HaveNoDependencyOnConstraint(
		string it,
		ExpectationGrammars grammars,
		string expected,
		StringEqualityOptions options)
		: ConstraintResult.WithValue<IEnumerable<Assembly?>>(grammars),
			IValueConstraint<IEnumerable<Assembly?>>
	{
		public ConstraintResult IsMetBy(IEnumerable<Assembly?> actual)
		{
			Actual = actual;
			Outcome = actual.All(assembly => 
				assembly?.GetReferencedAssemblies().Any(dep => options.AreConsideredEqual(dep.Name, expected)) != true)
				? Outcome.Success
				: Outcome.Failure;
			return this;
		}

		protected override void AppendNormalExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("all have no dependency on ").Append(options.GetExpectation(expected, Grammars));

		protected override void AppendNormalResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" contained assemblies with dependency ");
			Formatter.Format(stringBuilder,
				Actual?.Where(assembly => 
					assembly?.GetReferencedAssemblies().Any(dep => options.AreConsideredEqual(dep.Name, expected)) == true),
				FormattingOptions.Indented(indentation));
		}

		protected override void AppendNegatedExpectation(StringBuilder stringBuilder, string? indentation = null)
			=> stringBuilder.Append("not all have no dependency on ").Append(options.GetExpectation(expected, Grammars));

		protected override void AppendNegatedResult(StringBuilder stringBuilder, string? indentation = null)
		{
			stringBuilder.Append(it).Append(" only contained assemblies without dependency ");
			Formatter.Format(stringBuilder,
				Actual?.Where(assembly => 
					assembly?.GetReferencedAssemblies().Any(dep => options.AreConsideredEqual(dep.Name, expected)) != true),
				FormattingOptions.Indented(indentation));
		}
	}
}
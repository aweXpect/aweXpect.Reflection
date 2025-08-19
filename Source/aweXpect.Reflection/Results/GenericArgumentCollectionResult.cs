using System;
using aweXpect.Core;
using aweXpect.Options;
using aweXpect.Reflection.Helpers;
using aweXpect.Reflection.Options;
using aweXpect.Results;

namespace aweXpect.Reflection.Results;

/// <summary>
///     Additional constraints on a generic argument collection.
/// </summary>
public class GenericArgumentCollectionResult<TThat>(
	ExpectationBuilder expectationBuilder,
	IThat<TThat> subject,
	CollectionIndexOptions collectionIndexOptions,
	GenericArgumentFilterOptions genericArgumentFilterOptions)
	: AndOrResult<TThat, IThat<TThat>>(expectationBuilder, subject),
		IOptionsProvider<CollectionIndexOptions>,
		IOptionsProvider<GenericArgumentFilterOptions>
{
	private readonly ExpectationBuilder _expectationBuilder = expectationBuilder;
	private readonly IThat<TThat> _subject = subject;

	/// <inheritdoc cref="IOptionsProvider{CollectionIndexOptions}.Options" />
	CollectionIndexOptions IOptionsProvider<CollectionIndexOptions>.Options => collectionIndexOptions;

	/// <inheritdoc cref="IOptionsProvider{GenericArgumentFilterOptions}.Options" />
	GenericArgumentFilterOptions IOptionsProvider<GenericArgumentFilterOptions>.Options => genericArgumentFilterOptions;

	/// <summary>
	///     …at the given <paramref name="index" />.
	/// </summary>
	public GenericArgumentCollectionAtIndexResult<TThat> AtIndex(int index)
	{
		collectionIndexOptions.SetMatch(new HasGenericArgumentAtIndexMatch(index));
		return new GenericArgumentCollectionAtIndexResult<TThat>(_expectationBuilder, _subject,
			collectionIndexOptions);
	}

	/// <summary>
	///     …with the generic argument of type <typeparamref name="T" />.
	/// </summary>
	public GenericArgumentCollectionResult<TThat> WithGenericParameter<T>()
	{
		Type argumentType = typeof(T);
		genericArgumentFilterOptions.AddPredicate(t => t == argumentType,
			() => $"of type {Formatter.Format(argumentType)}");
		return this;
	}

	/// <summary>
	///     …with the specified <paramref name="count" /> of generic arguments.
	/// </summary>
	public GenericArgumentCollectionResult<TThat> WithCount(int count)
	{
		genericArgumentFilterOptions.AddCountPredicate(c => c == count,
			() => $"with {count} generic {(count == 1 ? "argument" : "arguments")}");
		return this;
	}
}
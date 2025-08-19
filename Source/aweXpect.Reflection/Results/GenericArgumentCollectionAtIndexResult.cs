using aweXpect.Core;
using aweXpect.Options;
using aweXpect.Results;

namespace aweXpect.Reflection.Results;

/// <summary>
///     Additional constraints on a generic argument collection with a generic argument at a specific index.
/// </summary>
public class GenericArgumentCollectionAtIndexResult<TThat>(
	ExpectationBuilder expectationBuilder,
	IThat<TThat> subject,
	CollectionIndexOptions collectionIndexOptions)
	: AndOrResult<TThat, IThat<TThat>>(expectationBuilder, subject),
		IOptionsProvider<CollectionIndexOptions>
{
	/// <inheritdoc cref="IOptionsProvider{CollectionIndexOptions}.Options" />
	CollectionIndexOptions IOptionsProvider<CollectionIndexOptions>.Options => collectionIndexOptions;
}
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace aweXpect.Formatting;

/// <summary>
///     Formatting extensions for <see cref="Assembly" />.
/// </summary>
public static class ReflectionFormatting
{
	/// <summary>
	///     Returns the according to the <paramref name="options" /> formatted <paramref name="value" />.
	/// </summary>
	public static string Format(
		this ValueFormatter formatter,
		Assembly? value,
		FormattingOptions? options = null)
		=> value?.FullName ?? ValueFormatter.NullString;

	/// <summary>
	///     Appends the according to the <paramref name="options" /> formatted <paramref name="value" />
	///     to the <paramref name="stringBuilder" />
	/// </summary>
	public static void Format(
		this ValueFormatter formatter,
		StringBuilder stringBuilder,
		Assembly? value,
		FormattingOptions? options = null)
		=> stringBuilder.Append(value?.FullName ?? ValueFormatter.NullString);
}

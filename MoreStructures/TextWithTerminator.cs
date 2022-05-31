using MoreStructures.Utilities;
using System.Collections;

namespace MoreStructures;

/// <summary>
/// A text string with a terminator character, not present in the text.
/// </summary>
/// <param name="Text">A text string, of any length (including the empty string).</param>
/// <param name="Terminator">
/// A terminator character, not present in the text. If not specified <see cref="DefaultTerminator"/> is used.
/// </param>
/// <param name="ValidateInput">
/// Whether the input, and in particular <see cref="Text"/> should be validated, while this object is created.
/// Validation takes O(n) time, where n = number of chars in <see cref="Text"/> and can be an heavy operation.
/// </param>
/// <remarks>
///     <para>
///     A terminator-terminated text is required by data structures like Suffix Tries, Trees or Arrays. 
///     </para>
///     <para>
///     This object provides type safety, as it allows to tell apart terminator-terminated strings from generic ones.
///     Consistently using <see cref="TextWithTerminator"/>, rather than <see cref="string"/>, in all library 
///     functionalities ensures that the invariant of a terminator-terminated string is always respected.
///     </para>
///     <para>
///     Most string-related functionalities provided by <see cref="TextWithTerminator"/>, such as <see cref="Length"/> 
///     and <see cref="this[Index]"/>, as well as <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/> support, 
///     are delegated to the underlying string.
///     </para>
/// </remarks>
public record TextWithTerminator(
    IEnumerable<char> Text, 
    char Terminator = TextWithTerminator.DefaultTerminator,
    bool ValidateInput = true)
    : IValueEnumerable<char>
{
    // Wrapped into a value enumerable to preserve value equality.
    private readonly IEnumerable<char> TextAndTerminator = Text.Append(Terminator).AsValue();

    /// <summary>
    /// A selector of a part of a <see cref="TextWithTerminator"/> or <see cref="RotatedTextWithTerminator"/>.
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// Extract the substring identified by this selector, out of the provided <see cref="TextWithTerminator"/>.
        /// </summary>
        /// <param name="text">The text with terminator, to extract a substring of.</param>
        /// <returns>A substring, whose length depends on the selector.</returns>
        string Of(TextWithTerminator text);

        /// <summary>
        /// Extract the substring identified by this selector, out of the provided 
        /// <see cref="RotatedTextWithTerminator"/>.
        /// </summary>
        /// <param name="text">The text with terminator, to extract a substring of.</param>
        /// <returns>A substring, whose length depends on the selector.</returns>
        string OfRotated(RotatedTextWithTerminator text);
    }

    /// <summary>
    /// The special character used as a default terminator for the text to build the Suffix Tree of, when no custom 
    /// terminator is specified. Should not be present in the text.
    /// </summary>
    public const char DefaultTerminator = '$';

    /// <summary>
    /// <inheritdoc cref="TextWithTerminator" path="/param[@name='Text']"/>
    /// </summary>
    /// <remarks>
    /// Wrapped into a <see cref="IValueEnumerable{T}"/> to preserve value equality.
    /// </remarks>
    public IEnumerable<char> Text { get; init; } = Text.AsValue();

    /// <summary>
    /// <inheritdoc cref="TextWithTerminator" path="/param[@name='Terminator']"/>
    /// </summary>
    public char Terminator { get; init; } =
        !ValidateInput || !Text.Contains(Terminator) 
        ? Terminator 
        : throw new ArgumentException($"{nameof(Terminator)} shouldn't be included in {nameof(Text)}");

    /// <summary>
    /// Select a part of this text by the provided selector.
    /// </summary>
    /// <param name="selector">Any selector acting on a <see cref="TextWithTerminator"/>.</param>
    /// <returns>A string containing the selected part.</returns>
    public string this[ISelector selector] => selector.Of(this);

    /// <summary>
    /// Select a part of this text by the provided range (start index included, end index excluded).
    /// </summary>
    /// <param name="range">The range applied to the underlying string.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of chars containing the selected part.</returns>
    public IEnumerable<char> this[Range range] => TextAndTerminator.Take(range).AsValue();

    /// <summary>
    /// Select a part of this text by the provided index (either w.r.t. the start or to the end of the text).
    /// </summary>
    /// <param name="index">The index applied to the underlying string.</param>
    /// <returns>A char containing the selected part.</returns>
    public char this[Index index] => TextAndTerminator.ElementAt(index);

    /// <summary>
    /// The total length of this text, including the terminator.
    /// </summary>
    public int Length => TextAndTerminator.Count();

    /// <summary>
    /// Whether this text starts with the provided suffix.
    /// </summary>
    /// <param name="prefix">A terminator-included <see cref="IEnumerable{T}"/> of <see cref="char"/>.</param>
    /// <returns>True if this text starts by the prefix.</returns>
    public bool StartsWith(IEnumerable<char> prefix) => TextAndTerminator.Take(prefix.Count()).SequenceEqual(prefix);

    /// <summary>
    /// Whether this text ends with the provided suffix.
    /// </summary>
    /// <param name="suffix">A terminator-included <see cref="IEnumerable{T}"/> of <see cref="char"/>.</param>
    /// <returns>True if this text ends by the suffix.</returns>
    public bool EndsWith(IEnumerable<char> suffix) => TextAndTerminator.TakeLast(suffix.Count()).SequenceEqual(suffix);

    /// <summary>
    /// Returns an enumerator that iterates through the collection of chars of the underlying <see cref="Text"/> 
    /// string, including the <see cref="Terminator"/> char.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public IEnumerator<char> GetEnumerator() => TextAndTerminator.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the collection of chars of the underlying <see cref="Text"/> 
    /// string, including the <see cref="Terminator"/> char.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)TextAndTerminator).GetEnumerator();
}
